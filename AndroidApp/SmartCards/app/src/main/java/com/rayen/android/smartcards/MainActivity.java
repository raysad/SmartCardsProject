package com.rayen.android.smartcards;

import android.content.Context;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    //Shared Prefs name
    private final String MY_PREF_NAME = "SmartCards";

    EditText textV1, textV2, textV3;
    Spinner dataType1, dataType2, dataType3;
    Spinner encoding1, encoding2, encoding3;

    //initialize encoding spinners
    private int[] spinnerInit;

    Context context;
    Button save;


    //List to store data in DataType structure
    private List<Datatype> myData;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        textV1 = findViewById(R.id.text1);
        textV2 = findViewById(R.id.text2);
        textV3 = findViewById(R.id.text3);
        dataType1 = findViewById(R.id.type1);
        dataType2 = findViewById(R.id.type2);
        dataType3 = findViewById(R.id.type3);
        encoding1 = findViewById(R.id.typeData1);
        encoding2 = findViewById(R.id.typeData2);
        encoding3 = findViewById(R.id.typeData3);

        spinnerInit = new int[]{0, 0, 0};

        save = findViewById(R.id.buttonSave);

        //Update button
        save.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //Count data number
                int chenges = 0;
                SharedPreferences.Editor editor = getSharedPreferences(MY_PREF_NAME, MODE_PRIVATE).edit();
                if(textV1.getText().length() != 0){
                    chenges++;
                    //Update shared prefs
                    Log.d("DEBUG", "OK");
                    editor.putString("data1", textV1.getText().toString());
                    Log.d("DEBUG", "Text = " + textV1.getText().toString());
                    editor.putInt("type1", dataType1.getSelectedItemPosition());
                    editor.putInt("typeData1", encoding1.getSelectedItemPosition() + 1);
                    editor.apply();
                }
                if(textV2.getText().length() != 0){
                    chenges++;
                    //Update shared prefs
                    editor.putString("data2", textV2.getText().toString());
                    editor.putInt("type2", dataType2.getSelectedItemPosition());
                    editor.putInt("typeData2", encoding2.getSelectedItemPosition() + 1);
                    editor.apply();
                }
                if(textV3.getText().length() != 0){
                    chenges++;
                    //Update shared prefs
                    editor.putString("data3", textV3.getText().toString());
                    editor.putInt("type3", dataType3.getSelectedItemPosition());
                    editor.putInt("typeData3", encoding3.getSelectedItemPosition() + 1);
                    editor.apply();
                }
                //Update the attribute changed to force the Service to reread the values
                editor.putInt("changed", 1);
                //Update data number
                editor.putInt("number", chenges);
                editor.apply();
                Toast.makeText(context, "Data updated", Toast.LENGTH_SHORT).show();

            }
        });

        //the adapter for each dataType spinner
        ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(this,
                R.array.data_type, android.R.layout.simple_spinner_item);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        context = this;
        dataType1.setAdapter(adapter);
        //A listener for each spinner to update Encoding spinner
        dataType1.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {

                if (position == 0){ //URI
                    Log.d("DEBUG", "URI change changed");
                    ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(context,
                                R.array.URI_type, android.R.layout.simple_spinner_item);
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    encoding1.setAdapter(adapter);
                }else { //TEXT
                    Log.d("DEBUG", "TEXT change changed");
                    ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(context,
                                R.array.text_type, android.R.layout.simple_spinner_item);
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    encoding1.setAdapter(adapter);
                }
                if (spinnerInit[0] != 0) {
                    encoding1.setSelection(spinnerInit[0]);
                    spinnerInit[0] = 0;
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
        dataType2.setAdapter(adapter);
        dataType2.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if (position == 0){
                    ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(context,
                            R.array.URI_type, android.R.layout.simple_spinner_item);
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    encoding2.setAdapter(adapter);
                }else {
                    ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(context,
                            R.array.text_type, android.R.layout.simple_spinner_item);
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    encoding2.setAdapter(adapter);
                }
                if (spinnerInit[1] != 0) {
                    encoding2.setSelection(spinnerInit[1]);
                    spinnerInit[1] = 0;
                }

            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        dataType3.setAdapter(adapter);
        dataType3.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if (position == 0){
                    ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(context,
                            R.array.URI_type, android.R.layout.simple_spinner_item);
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    encoding3.setAdapter(adapter);
                }else {
                    ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(context,
                            R.array.text_type, android.R.layout.simple_spinner_item);
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    encoding3.setAdapter(adapter);
                }
                if (spinnerInit[2] != 2) {
                    encoding3.setSelection(spinnerInit[2]);
                    spinnerInit[2] = 0;
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });

        //Get the shared preferences data to initialize the edit text and spinners
        SharedPreferences sharedPreferences = getSharedPreferences(MY_PREF_NAME, MODE_PRIVATE);

        //Get the number of saved data
        int nbrData = sharedPreferences.getInt("number", 0);
        Log.d("DEBUG", "nmr = " + nbrData);
        myData = new ArrayList<>();

        //Fill the list with the data from the shared preferences
        for (int i =1; i< nbrData +1 ; i++) {
            Datatype myDataType = new Datatype();
            myDataType.setData(sharedPreferences.getString("data" + i, null));
           // Log.d("DEBUG", sharedPreferences.getString("data" + i, null));
            myDataType.setDataType(sharedPreferences.getInt("type" + i, 0));
            myDataType.setDataEncode(sharedPreferences.getInt("typeData" + i, 0));
            myData.add(myDataType);
        }

        //Update the Edit texts and the spinners
        if (nbrData > 0){
            Log.d("DEBUG", "Data: " + myData.get(0).getData() + " Type: " + myData.get(0).getDataType() +
                    " Encoding: " + myData.get(0).getDataEncode());
            textV1.setText(myData.get(0).getData());
            dataType1.setSelection(myData.get(0).getDataType());
            spinnerInit[0] = myData.get(0).getDataEncode() -1;
            if (nbrData> 1){
                textV2.setText(myData.get(1).getData());
                dataType2.setSelection(myData.get(1).getDataType());
                spinnerInit[1] = myData.get(1).getDataEncode()-1;
                if (nbrData > 2) {
                    textV3.setText(myData.get(2).getData());
                    dataType3.setSelection(myData.get(2).getDataType());
                    spinnerInit[2] = myData.get(2).getDataEncode()-1;
                }
            }
        }

    }


    //Data structure
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
}
