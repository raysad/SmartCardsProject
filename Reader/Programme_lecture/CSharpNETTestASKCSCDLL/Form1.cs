using System; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSharpNETTestASKCSCDLL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /*----------------------------------------------------------*/
        //fonction pour cree les commandes de selection 
        /*----------------------------------------------------------*/

        public Byte[] CreateCmd(Byte p2 , Byte le)
        {
            Byte[] readBinaryCmd = new Byte[] { 0x00/*CLA*/, 0xB0/*IWS*/, 0x00/*P1*/, p2/*P2*/ , le/*LE*/ };//commande
            return readBinaryCmd;
        }

        /*----------------------------------------------------------*/
        // fonction pour lire les records et extraire les champs de la commande de retour 
        /*----------------------------------------------------------*/

        public void ReadRecord (Byte[] byBufOut, int len)
        {
            //Console.WriteLine("len = " + len);
            
            string firstByte = Convert.ToString(byBufOut[1], 2).PadLeft(8, '0');
            
            Console.WriteLine(firstByte);
            Console.WriteLine(byBufOut[1]);
            string mb = firstByte.Substring(0, 1);
            string me = firstByte.Substring(1, 1);
            string cf = firstByte.Substring(2, 1);
            string sr = firstByte.Substring(3, 1);
            string il = firstByte.Substring(4, 1);
            string tnf = firstByte.Substring(5, 3);

            Console.WriteLine("Mb = " + mb + " \n me = " + me + " \n cf = " + cf + " \n sr = " + sr + " \n il = " + il + " \n tnf = " + tnf);

            string typeLength = byBufOut[2].ToString();

            Console.WriteLine(" \n typeLength = " + typeLength);

            string payloadLength = byBufOut[3].ToString();

            Console.WriteLine(" \n payloadLength = " + payloadLength);



            int offsetID = 0;

            int offsetType = 0;

            Byte type2 =0x00;




            /*----------------------------------------------------------*/
            // Si le payload contient un ID on decale la lecture du tableau
            /*----------------------------------------------------------*/

            if (il == "1")
            {
                string idLength = byBufOut[4].ToString();
                Console.WriteLine(" \n idLength = " + idLength);

                string id = byBufOut[6].ToString();
                Console.WriteLine(" \n id = " + id);
                offsetID = 1;
            }
            else
            {
                
                Console.WriteLine(" \n  pas d'ID ");
            }

            Byte type = byBufOut[4+offsetID];
           

            if (typeLength == "2")
            {
                type2 = byBufOut[4 + offsetID+1];
                
                offsetType = 1;
            }

            /*----------------------------------------------------------*/
            // pour les smart Poster
            /*----------------------------------------------------------*/


            if ((type == 0x53) && (type2 == 0x70))
            {
                ReadSmartPoster(byBufOut, len);
            }

            /*----------------------------------------------------------*/
            // pour les reste des types 
            /*----------------------------------------------------------*/
            else
            {               
                SplitPayload(byBufOut, len, mb, me);
            }

        }


        // fonction pour lire les SMART POSTER 
        public void ReadSmartPoster (Byte[] byBufOut, int len )
        {

            

            Byte[] payload = new Byte[64];


            Console.WriteLine("______________________________________________________");
            Console.WriteLine("SMART POSTER FN");
            Console.WriteLine("______________________________________________________");

            Array.Copy(byBufOut, 0x05, payload, 0, 50);
            Console.WriteLine(BitConverter.ToString(payload).Replace("-", string.Empty));
            Console.WriteLine("______________________________________________________");

            /*----------------------------------------------------------*/
            // Appel fonction readRecord() pour lire le reste du payload 
            /*----------------------------------------------------------*/

            ReadRecord(payload,payload[3]);

        }


        // fonction pour ex 
        public void SplitPayload(Byte[] record, int len, String mb, String me)
        {
            /*----------------------------------------------------------*/
            // en cas du dernier message
            /*----------------------------------------------------------*/


            if (me == "1")
            {
                ReadPayload(record);
                Console.WriteLine("______________________________________________________");
                Console.WriteLine("LAST MSG");
                Console.WriteLine("______________________________________________________");
                Console.WriteLine(BitConverter.ToString(record).Replace("-", string.Empty));
                Console.WriteLine("______________________________________________________");

            }

            /*----------------------------------------------------------*/
            //si ce n'est pas le dernier message on lit la premier partie et on envoi le reste à la fonction readRecord() 
            /*----------------------------------------------------------*/


            else if (me == "0")
            {
                
                Console.WriteLine("______________________________________________________");
                Console.WriteLine("STILL MSG");
                Console.WriteLine("______________________________________________________");
                Console.WriteLine(BitConverter.ToString(record).Replace("-", string.Empty));
                Console.WriteLine("______________________________________________________");

                Byte[] payload = new Byte[64];

             


                Array.Copy(record, 0x00, payload, 0, record[3] + record[2] + 0x04);
                Console.WriteLine("______________________________________________________");
                Console.WriteLine(BitConverter.ToString(payload).Replace("-", string.Empty));
                Console.WriteLine("______________________________________________________");
                ReadPayload(payload);
				
				Byte[] payload2 = new Byte[64];

                Array.Copy(record, record[3] + record[2] + 0x03, payload2, 0 , 36);
                /*----------------------------------------------------------*/
                // Appel fonction readRecord() pour lire le reste du payload
                /*----------------------------------------------------------*/

                ReadRecord(payload2,20);
				
				

            }
        }
        /*----------------------------------------------------------*/
        // fonction pour lire le contenu du payload selon son format
        /*----------------------------------------------------------*/

        public void ReadPayload(Byte[] payload)
        {
            
            if (payload[4]== 0x55)
            {
                Console.WriteLine("______________________________________________________");
                Console.WriteLine("URI");
                Console.WriteLine("______________________________________________________");

                String msg="";

                if (payload[5]==0x01)
                {
                    msg = msg + "http://wwww.";
                }

                else if (payload[5] == 0x02)
                {
                    msg = msg + "https://wwww.";
                }
                else if (payload[5] == 0x03)
                {
                    msg = msg + "http://";
                }
                else if (payload[5] == 0x04)
                {
                    msg = msg + "https://";
                }

                Byte[] textBuf = new Byte[payload[3] - 1];
                Array.Copy(payload, 0x06, textBuf, 0, payload[3] - 1);
                string text = Encoding.ASCII.GetString(textBuf);

                msg = msg + text;
                Console.WriteLine(msg);
                Console.WriteLine("______________________________________________________");

            }
            else if (payload[4] == 0x54)
            {
                Console.WriteLine("______________________________________________________");
                Console.WriteLine("TEXT");
                Console.WriteLine("______________________________________________________");
                Byte[] textBuf = new Byte[payload[3] - 1];
                Array.Copy(payload, 0x06, textBuf, 0, payload[3] - 1);
                string text = Encoding.ASCII.GetString(textBuf);

                Console.WriteLine(text);
                Console.WriteLine("______________________________________________________");
            }
            if (payload[2] == 0x00)
            {
                Console.WriteLine("______________________________________________________");
                Console.WriteLine("DATA");
                Console.WriteLine("______________________________________________________");
                Byte[] textBuf = new Byte[payload[3] - 1];
                Array.Copy(payload, 0x05, textBuf, 0, payload[3] - 1);
                string text = Encoding.ASCII.GetString(textBuf);

                Console.WriteLine(text);
                Console.WriteLine("______________________________________________________");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AskReaderLib.CSC.sCARD_SearchExtTag SearchExtender;
            int Status;
            byte[] ATR;
            ATR = new byte[200];
            int lgATR;
            lgATR = 200;
            int Com=0;
            int SearchMask;

            txtCom.Text = "";
            txtCard.Text = "";
            

            try
			{
				AskReaderLib.CSC.SearchCSC();
				// user can also use line below to speed up coupler connection
				//AskReaderLib.CSC.Open ("COM2");

				// Define type of card to be detected: number of occurence for each loop
				SearchExtender.CONT = 0;
				SearchExtender.ISOB = 2;
				SearchExtender.ISOA = 2;
				SearchExtender.TICK = 1;
				SearchExtender.INNO = 2;
				SearchExtender.MIFARE = 0;
				SearchExtender.MV4k = 0;
				SearchExtender.MV5k = 0;
				SearchExtender.MONO = 0;

				// Define type of card to be detected
				Status = AskReaderLib.CSC.CSC_EHP_PARAMS_EXT(1, 1, 0, 0, 0, 0, 0, 0, null, 0, 0);

                if (Status == AskReaderLib.CSC.RCSC_Ok)
                {
                    SearchMask = AskReaderLib.CSC.SEARCH_MASK_ISOB | AskReaderLib.CSC.SEARCH_MASK_ISOA;
                    Status = AskReaderLib.CSC.SearchCardExt(ref SearchExtender, SearchMask, 1, 20, ref Com, ref lgATR, ATR);

                    if (Status == AskReaderLib.CSC.RCSC_Ok)
                    {
                        Console.WriteLine("Ok");
                    }
                }
                Byte[] ndefSelectCmd = new Byte[7];
				
				
                if (Com == 2)
				{
					txtCard.Text = "ISO14443A-4 no Calypso";
					Byte[] selectAppCmd = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x04/*P1*/, 0x00/*P2*/, 0x07/*lc*/, 0xD2, 0x76, 0x00, 0x00, 0x85, 0x01, 0x01/*data*/, 0x00/*le*/ };//commande 
                    Byte[] selectFileCmd = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x00/*P1*/, 0x0C/*P2*/, 0x02/*lc*/, 0xE1, 0x03 };//commande 

                    

                    Byte[] readBinaryCmd = CreateCmd(0x00,0x0F);

                    int iLenOUT = 0;
					Byte[] byBufOut = new Byte[64];
					
					Status = AskReaderLib.CSC.CSC_ISOCommand(selectAppCmd, selectAppCmd.Length, byBufOut, ref iLenOUT);

					if (Status == AskReaderLib.CSC.RCSC_Ok)
						{
						
                        Console.WriteLine("\n****\n");
                        Console.WriteLine("Select application");
						Console.WriteLine(BitConverter.ToString(byBufOut).Replace("-", string.Empty));
						
						Array.Clear(byBufOut, 0, byBufOut.Length);


                        Status = AskReaderLib.CSC.CSC_ISOCommand(selectFileCmd, selectFileCmd.Length, byBufOut, ref iLenOUT);

                        if (Status == AskReaderLib.CSC.RCSC_Ok)
                        {
                            Console.WriteLine("\n****\n");
                            Console.WriteLine("\nSelect File\n");
                            Console.WriteLine(BitConverter.ToString(byBufOut).Replace("-", string.Empty));
                            Console.WriteLine("\n****\n");
							Array.Clear(byBufOut, 0, byBufOut.Length);

                            int status = AskReaderLib.CSC.CSC_ISOCommand(readBinaryCmd, readBinaryCmd.Length, byBufOut,ref iLenOUT);
                            if (status == AskReaderLib.CSC.RCSC_Ok)
                            {
                                Console.WriteLine("\n****\n");
                                Console.WriteLine("Read File");
                                Console.WriteLine("\n****\n");
                                Console.WriteLine(BitConverter.ToString(byBufOut).Replace("-", string.Empty));
							}	

                            //commande selection NDEF File
							ndefSelectCmd = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x00/*P1*/, 0x0C/*P2*/, 0x02/*lc*/, byBufOut[10]/*0xE1*/, byBufOut[11] /*0xE1*/};//commande 
							
							Console.WriteLine(BitConverter.ToString(ndefSelectCmd).Replace("-", string.Empty));
                            Console.WriteLine("_________________________________________________");
							Array.Clear(byBufOut, 0, byBufOut.Length);
							Status = AskReaderLib.CSC.CSC_ISOCommand(ndefSelectCmd, ndefSelectCmd.Length, byBufOut, ref iLenOUT);
							if (Status == AskReaderLib.CSC.RCSC_Ok)
							{
								Console.WriteLine("Select done");
								Console.WriteLine(BitConverter.ToString(byBufOut).Replace("-", string.Empty));
								Array.Clear(byBufOut, 0, byBufOut.Length);

                                
                                Byte[] readBinaryCmd2 = CreateCmd(0x00, 0x02);
                                Status = AskReaderLib.CSC.CSC_ISOCommand(readBinaryCmd2, readBinaryCmd.Length, byBufOut, ref iLenOUT);
								if (Status == AskReaderLib.CSC.RCSC_Ok)
								{
									Console.WriteLine("Read NDEF size" );
									Console.WriteLine(BitConverter.ToString(byBufOut).Replace("-", string.Empty));
									
								}
                                
                                Byte[] readBinaryCmd3 = CreateCmd(0x02, byBufOut[2]);
                                Byte len = byBufOut[2];
                                
								Console.WriteLine(BitConverter.ToString(readBinaryCmd3).Replace("-", string.Empty));

								Array.Clear(byBufOut, 0, byBufOut.Length);
								
								Status = AskReaderLib.CSC.CSC_ISOCommand(readBinaryCmd3, readBinaryCmd3.Length, byBufOut, ref iLenOUT);
								if (Status == AskReaderLib.CSC.RCSC_Ok)
								{
									Console.WriteLine("Read B NDEF");
									Console.WriteLine(BitConverter.ToString(byBufOut).Replace("-", string.Empty));

                                    /*----------------------------------------------------------*/
                                    // Appel ReadRecord()
                                    /*----------------------------------------------------------*/
                                    ReadRecord(byBufOut,len);


                                    /*----------------------------------------------------------*/

                                    

								}
									
							}                         
                            
                        }
                        
                    }
                }

				else if (Com == 3)
					txtCard.Text = "INNOVATRON";
				else if (Com == 4)
					txtCard.Text = "ISOB14443B-4 Calypso";
				else if (Com == 5)
					txtCard.Text = "Mifare";
				else if (Com == 6)
					txtCard.Text = "CTS or CTM";
				else if (Com == 8)
					txtCard.Text = "ISO14443A-3 ";
				else if (Com == 9)
					txtCard.Text = "ISOB14443B-4 Calypso";
				else if (Com == 12)
					txtCard.Text = "ISO14443A-4 Calypso";
				else if (Com == 0x6F)
					txtCard.Text = "Card not found";
				else
					txtCard.Text = "";
        
            }
            catch
            {
                MessageBox.Show("Error on trying do deal with reader");
            }
            AskReaderLib.CSC.Close();
        }
    }
}