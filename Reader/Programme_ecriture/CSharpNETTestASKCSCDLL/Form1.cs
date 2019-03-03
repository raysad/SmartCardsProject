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
				;
				
                if (Com == 2)
				{
					txtCard.Text = "ISO14443A-4 no Calypso";
					Byte[] selectAppCmd = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x04/*P1*/, 0x00/*P2*/, 0x07/*lc*/, 0xD2, 0x76, 0x00, 0x00, 0x85, 0x01, 0x01/*data*/, 0x00/*le*/ };//commande 
                    Byte[] selectFileCmd = new Byte[] { 0x00/*CLA*/, 0xA4/*IWS*/, 0x00/*P1*/, 0x0C/*P2*/, 0x02/*lc*/, 0xE1, 0x03 };//commande 

                    

                    

                    int iLenOUT = 0;
					Byte[] byBufOut = new Byte[64];
					Byte[] byBufOut2 = new Byte[64];
					
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

                            Byte[] writeFirst = new Byte[] {0x00, 0xD6, 0x00, 0x00, 0x10, 0x00, 0x31, 0x91, 0x01, 0x0A, 0x55, 0x01, 0x61, 0x70, 0x70, 0x6C, 0x65, 0x2E, 0x63, 0x6F, 0x6D };
							Byte[] writeSecond = new Byte[] {0x00, 0xD6, 0x00, 0x10, 0x23, 0x11, 0x01, 0x14, 0x54, 0x02, 0x66, 0x72, 0x4C, 0x61, 0x20, 0x62, 0x65, 0x6C, 0x6C, 0x65, 0x20, 0x68, 0x69, 0x73, 0x74, 0x6F, 0x69, 0x72, 0x65, 0x51, 0x00, 0x08, 0x50, 0x4F, 0x4C, 0x59, 0x54, 0x45, 0x43, 0x48 };


							int statusTest = AskReaderLib.CSC.CSC_ISOCommand(writeFirst, writeFirst.Length, byBufOut2, ref iLenOUT);

                            if (statusTest == AskReaderLib.CSC.RCSC_Ok)
                            {
                                Console.WriteLine("Update OK");
                                Console.WriteLine(BitConverter.ToString(byBufOut2).Replace("-", string.Empty));
                            }

                            statusTest = AskReaderLib.CSC.CSC_ISOCommand(writeSecond, writeSecond.Length, byBufOut2, ref iLenOUT);

                            if (statusTest == AskReaderLib.CSC.RCSC_Ok)
                            {
                                Console.WriteLine("Update2 OK");
                                Console.WriteLine(BitConverter.ToString(byBufOut2).Replace("-", string.Empty));
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