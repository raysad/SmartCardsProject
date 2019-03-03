package com.rayen.android.smartcards;

import android.content.SharedPreferences;
import android.nfc.NdefMessage;
import android.nfc.NdefRecord;
import android.nfc.cardemulation.HostApduService;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;


import java.lang.reflect.Array;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;


public class NFCService extends HostApduService {

    //Pref Nale
    private final String MY_PREF_NAME = "SmartCards";

    //List to store all data
    private List<Datatype> myData;

    //Final NDEF File
    private byte[] myNDEFFile;

    //List to store data in hex format
    private List<byte[]> myDataHex;


    //Errors
    private final byte[] errorCLA = new byte[]{(byte) 0x6E, (byte) 0x00};
    private final byte[] errorINS = new byte[]{(byte) 0x6D, (byte) 0x00};
    private final byte[] errorLC = new byte[]{(byte) 0x67, (byte) 0x00};
    private final byte[] errorLE = new byte[]{(byte) 0x6C, (byte) 0x00};
    private final byte[] errorAID = new byte[]{(byte) 0x6A, (byte) 0x82};
    private final byte[] errorEtat = new byte[]{(byte) 0x69, (byte) 0x86};
    private final byte[] errorP1P2Select = new byte[]{(byte) 0x6A, (byte) 0x86};
    private final byte[] errorP1P2R_U = new byte[]{(byte) 0x6B, (byte) 0x00};
    private final byte[] errorOffsetLC = new byte[]{(byte) 0x6A, (byte) 0x87};
    private final byte[] errorOffsetLE = new byte[]{(byte) 0x6C, (byte) 0x00};
    private final byte[] errorAcess = new byte[]{(byte) 0x69, (byte) 0x85};

    private final byte[]  commandOk = new byte[]{(byte) 0x90, (byte) 0x00};

    private final byte[] INSValues = new byte[]{(byte) 0xA4, (byte) 0xB0, (byte) 0xD6};
    private final byte[] AppId = new byte[]{(byte) 0xD2, (byte) 0x76, (byte) 0x00, (byte) 0x00, (byte) 0x85, (byte) 0x01, (byte) 0x01};


//    private final static byte[] CC_FILE = new byte[]{0x00, 0x0f, 0x20, 0x00, 0x3b, 0x00, 0x34, 0x04, 0x06, (byte) 0x81, 0x01, 0x00, 0x32, 0x00, (byte) 0x00 };
    private final static byte[] CCFile_CCLen = new byte[]{0x00, 0x0f};
    private final static byte CCFile_MappingVersion = (byte) 0x20;
    private final static byte[] CCFile_MLe = new byte[]{0x00, 0x3B};
    private final static byte[] CCFile_MLc = new byte[]{0x00, 0x34};
    private final static byte[] CCFile_NDEFFile = new byte[]{ 0x04, 0x06, (byte) 0xE1, 0x04, 0x10, 0x00, 0x00, (byte) 0x00};

    private final byte[] NDEFFileLID = new byte[]{(byte) 0xE1, (byte) 0x04};

    private final byte[] testNdef = new byte[]{0x00, 0x0E, (byte) 0xD1, 0x01, 0x0A, 0x55, 0x01, 0x61, 0x70, 0x70, 0x6C, 0x65, 0x2E, 0x63, 0x6F, 0x6D};

    private boolean AppSelected;
    private boolean CCFileSelected;
    private boolean NDEFFileSelected;

    //Test if a byte is inside an array
    boolean isInside (byte val, byte[] valArray){
        boolean found = false;
        int index = 0;
        while (!found && index<valArray.length){
            if(valArray[index] == val)
                found = true;
        }
        return found;
    }

    //Get a sub array from index with length number of bytes
    public static byte[] subArray(byte[] data, int index, int length)
    {
        byte[] result;
        result = Arrays.copyOfRange(data, index, index + length);
        return result;
    }


    //Convert int to byte array
    private byte[] IntToByte(int a)
    {
        byte[] array = new byte[2];
        array[0] = (byte) ((a & 0xff00) / 256);
        array[1] = (byte) (a & 0xff);
        return array;
    }

    //Create Record header depends on his position in the NDEF File
    private byte getHeader(boolean first, boolean last) {
        if (first && last)
            return (byte) 0xD1;
        if (first && !last)
            return (byte) 0x91;
        if (!first && last)
            return (byte) 0x51;

        return (byte) 0x11;
    }

    @Override
    public void onCreate() {
        super.onCreate();

        AppSelected = false;
        CCFileSelected = false;
        NDEFFileSelected = false;

        myData = new ArrayList<>();
        myDataHex = new ArrayList<>();

        //Commented to work with testNdef
        //ndefCreate();


    }

    @Override
    public byte[] processCommandApdu(byte[] commandApdu, Bundle extras) {
        StringBuilder sb = new StringBuilder(commandApdu.length * 2);
        for(byte b: commandApdu)
            sb.append(String.format("%02x", b));
        Log.d("DEBUG", "Length = " + commandApdu.length + " Command = " +  sb);

        if(commandApdu[0] != (byte) (0x00)){
            return errorCLA;
        }
        if(commandApdu[1] == (byte) 0xA4){ //Select
           // Toast.makeText(this, "Select", Toast.LENGTH_SHORT).show();
            if((commandApdu[2] == (byte) 0x04) && (commandApdu[3] == (byte) 0x00)){ // Select App
         //       Toast.makeText(this, "App selected", Toast.LENGTH_SHORT).show();
                if((commandApdu[4] > (byte) 0x04) && (commandApdu[4] < (byte) 0x08)){ // LC OK
                    int lc = (int) commandApdu[4];
            //        Toast.makeText(this, "Lc OK : " + lc, Toast.LENGTH_SHORT).show();
                    if (Arrays.equals(subArray(AppId, 0, lc), subArray(commandApdu, 5, lc))) {
               //         Toast.makeText(this, "App OK ", Toast.LENGTH_SHORT).show();
                        if(commandApdu[5+lc] == (byte) 0x00){
                            AppSelected = true;
                            CCFileSelected = false;
                            NDEFFileSelected = false;
                         //   Toast.makeText(this, "Changed", Toast.LENGTH_SHORT).show();
                            return commandOk;

                        }else {
                            return errorLE;
                        }
                    }else {
                   //     Toast.makeText(this, "App error", Toast.LENGTH_SHORT).show();
                        return errorAID;
                    }
                }else {
                    return errorLC;
                }
            }else if ((commandApdu[2] == (byte) 0x00) && (commandApdu[3] == (byte) 0x0C)){// Select File
            //    Toast.makeText(this, "File Selected", Toast.LENGTH_SHORT).show();
                if (commandApdu[4] == (byte) 0x02) {
                    if ((commandApdu[5] == (byte) 0xE1) && (commandApdu[6] == (byte) 0x03)) { //Select CC File
                        if (AppSelected) {
                            CCFileSelected = true;
                            NDEFFileSelected = false;
                            return commandOk;
                        }else {
                            return errorEtat;
                        }
                    } else if ((commandApdu[5] == NDEFFileLID[0]) && (commandApdu[6] == NDEFFileLID[1])) { //Select NDEF File
                        if (AppSelected) {
                            NDEFFileSelected = true;
                            CCFileSelected = false;
                            return commandOk;
                        }else {
                            return errorEtat;
                        }
                    }else {
                        return errorAID;
                    }
                }else {
                    return errorLC;
                }
            }else{
                return errorP1P2Select;
            }


        }else if(commandApdu[1] == (byte) 0xB0){ // Read Binary
            if(CCFileSelected){
            //    Toast.makeText(this, "CCFile Selected", Toast.LENGTH_SHORT).show();
                int offset = (0x00ff & commandApdu[2]) * 256 + (0x00ff & commandApdu[3]);
                if (offset > 14) {
                    return errorP1P2R_U;
                }
                int le = (int) commandApdu[4];
//                int maxLe = (0x00ff & CCFile_MLe[0]) * 256 + (0x00ff & CCFile_MLe[1]);
//
//                if (le > maxLe || (le + offset)>4096){
//                    return errorOffsetLE;
//                }
                byte[] CCFile = getCCFile();
                int byteToRead = (le < (15 - offset)) ? le : (15 - offset);
                byte[] CCFileReturned = new byte[byteToRead];
                System.arraycopy(CCFile, offset, CCFileReturned, 0, byteToRead);

                byte[] returnedResponse = new byte[byteToRead + 2];
                System.arraycopy(CCFileReturned, 0, returnedResponse, 0, byteToRead);
                System.arraycopy(commandOk, 0, returnedResponse, byteToRead, 2);
                return returnedResponse;

            }else if (NDEFFileSelected){
             //   Toast.makeText(this, "NDEF Selected", Toast.LENGTH_SHORT).show();
                //ndefCreate();
                int offset = (0x00ff & commandApdu[2]) * 256 + (0x00ff & commandApdu[3]);
                if (offset > testNdef.length-1) {
                    return errorP1P2R_U;
                }
                int le = (int) commandApdu[4];
                int maxLe = (0x00ff & CCFile_MLe[0]) * 256 + (0x00ff & CCFile_MLe[1]);

                if (le > maxLe || (le + offset)>4096){
                    return errorOffsetLE;
                }

                int ndefLength = testNdef.length;
                int byteToRead = (le < (ndefLength - offset)) ? le : (ndefLength - offset);
                byte[] NDEFFileReturned = new byte[byteToRead];
                System.arraycopy(testNdef, offset, NDEFFileReturned, 0, byteToRead);

                byte[] returnedResponse = new byte[byteToRead + 2];
                System.arraycopy(NDEFFileReturned, 0, returnedResponse, 0, byteToRead);
                System.arraycopy(commandOk, 0, returnedResponse, byteToRead, 2);
                return returnedResponse;

            }else {
            //    Toast.makeText(this, "error etat", Toast.LENGTH_SHORT).show();
                return errorEtat;
            }

        }else if(commandApdu[1] == (byte) 0xD6){ //Update Binary
        //    Toast.makeText(this, "Update B", Toast.LENGTH_SHORT).show();
            int offset = (0x00ff & commandApdu[2]) * 256 + (0x00ff & commandApdu[3]);
            if (offset > 4095) {
                return errorP1P2R_U;
            }
            int lc = (int) commandApdu[4];
            if(lc != (commandApdu.length -5)){
                return errorOffsetLC;
            }
            //TODO Complete Update binary
            return commandOk;
        //    Toast.makeText(this, "Longeur comm = " + commandApdu.length, Toast.LENGTH_SHORT).show();

        }else {
            return errorINS;
        }

        //return FAILURE_SW;
    }

    //CCFile creator
    private byte[] getCCFile() {
        byte[] mArray = new byte[15];
        System.arraycopy(CCFile_CCLen, 0, mArray, 0, 2);
        mArray[2] = CCFile_MappingVersion;
        System.arraycopy(CCFile_MLe, 0, mArray, 3, 2);
        System.arraycopy(CCFile_MLc, 0, mArray, 5, 2);
        System.arraycopy(CCFile_NDEFFile, 0, mArray, 7, 8);
        return mArray;
    }

    class Datatype {

        public Datatype(){
            data = "";
        }

        public Datatype(String data, int dataType, int dataEncode) {
            this.data = data;
            this.dataType = dataType;
            this.dataEncode = dataEncode;
        }

        private String data;
        private int dataType;
        private int dataEncode;

        public String getData() {
            return data;
        }

        public void setData(String data) {
            this.data = data;
        }

        public int getDataType() {
            return dataType;
        }

        public void setDataType(int dataType) {
            this.dataType = dataType;
        }

        public int getDataEncode() {
            return dataEncode;
        }

        public void setDataEncode(int dataEncode) {
            this.dataEncode = dataEncode;
        }
    }

    //TODO Correct error
    private void ndefCreate(){
        SharedPreferences sharedPreferences = getSharedPreferences(MY_PREF_NAME, MODE_PRIVATE);
        int changed = sharedPreferences.getInt("changed", 1);
        if (changed != 0) {

            SharedPreferences.Editor editor = getSharedPreferences(MY_PREF_NAME, MODE_PRIVATE).edit();
            editor.putInt("changed", 0);
            editor.apply();

            int nbrData = sharedPreferences.getInt("number", 0);
            for (int i =0; i< nbrData; i++) {
                Datatype myDataType = new Datatype();
                myDataType.setData(sharedPreferences.getString("data" + i, null));
                if (myDataType.getData() == null) {
                    Log.d("DEBUG", "NULL");
                }
                myDataType.setDataType(sharedPreferences.getInt("type" + i, 0));
                myDataType.setDataEncode(sharedPreferences.getInt("typeData" + i, 0));
                myData.add(myDataType);
            }
            //String data1 = sharedPreferences.getString("data1", null);
            for (Datatype current :
                    myData) {
                Log.d("DEBUG", "Data : " + current.getData() + " Type : " + current.getDataType() + " Encode : " + current.getDataEncode());
                int index = myData.indexOf(current);
                boolean first = false;
                boolean last = false;
                if (index == 0) {
                    first = true;
                } else if (index == (nbrData - 1)) {
                    last = true;
                }
                if (current.getDataType() == 0) { //URI
                    byte[] dataHex = new byte[current.getData().length() + 5];
                    dataHex[0] = getHeader(first, last);
                    dataHex[1] = (byte) 0x01;
                    dataHex[2] = (byte) (current.getData().length() + 1);
                    dataHex[3] = (byte) 0x55;
                    dataHex[4] = (byte) current.getDataEncode();
                    byte[] payloadArray = current.getData().getBytes(StandardCharsets.UTF_8);
                    System.arraycopy(payloadArray, 0, dataHex, 5, payloadArray.length);
                    myDataHex.add(dataHex);

                } else { //text
                    byte[] dataHex = new byte[current.getData().length() + 7];
                    dataHex[0] = getHeader(first, last);
                    dataHex[1] = (byte) 0x01;
                    dataHex[2] = (byte) (current.getData().length() + 3);
                    dataHex[3] = (byte) 0x54;
                    dataHex[4] = (byte) 0x02;
                    byte[] textLang = new byte[2];
                    if (current.getDataEncode() == 1){
                        textLang[0] = (byte) 0x66;
                        textLang[1] = (byte) 0x72;
                    }else {
                        textLang[0] = (byte) 0x65;
                        textLang[1] = (byte) 0x6E;
                    }
                    System.arraycopy(textLang, 0, dataHex, 5, 2);
                    byte[] payloadArray = current.getData().getBytes(StandardCharsets.UTF_8);
                    System.arraycopy(payloadArray, 0, dataHex, 7, payloadArray.length);
                    myDataHex.add(dataHex);
                }
            }
            int ndefLength = 0;
            for (byte[] current :
                    myDataHex) {
                ndefLength += current.length;
            }

            //Log.d("DEBUG", "Len = " + ndefLength);
            Toast.makeText(this, "Len = " + ndefLength, Toast.LENGTH_SHORT).show();

            myNDEFFile = new byte[ndefLength + 2];

            myNDEFFile[0] = (byte) ((ndefLength & 0xff00) / 256);
            myNDEFFile[1] = (byte) (ndefLength & 0xff);

            int ndefOffset = 2;
            for (byte[] current :
                    myDataHex) {
                System.arraycopy(current, 0, myNDEFFile, ndefOffset, current.length);
                ndefOffset += current.length;
            }
        }
    }

    @Override
    public void onDeactivated(int reason) {

        AppSelected = false;
        CCFileSelected = false;
        NDEFFileSelected = false;

    }
}
