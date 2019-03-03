/*************************************************************************/
// Librairie d'adaptation du lecteur ASK RDR417 pour le framework .net 2.0
// Création MARTORANA Fabrice.
/*************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace AskReaderLib
{

    public static class CSC
    {

        public delegate void ReadCard(byte[] read); 

        public static string ToStringN(byte[] bytes)
        {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }

        static char[] hexDigits = {
        '0', '1', '2', '3', '4', '5', '6', '7',
        '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};


        #region MethodesASK
        [DllImport("Askcsc.dll", EntryPoint = "CSC_Open")]
        public static extern Int32 Open(string portCom);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_Close")]
        public static extern void Close();

        [DllImport("Askcsc.dll", EntryPoint = "CSC_GetUSBNumDevices")]
        public static extern Int32 GetUSBNumDevices(ref  Int32 numDevice);



        [DllImport("Askcsc.dll", EntryPoint = "CSC_VersionCSC")]
        public static extern Int32 VersionDLL(byte[] version);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_GetPCSCDeviceName")]
        public static extern Int32 GetPCSCDeviceName(Int32 numDevice, string sName);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_SearchCSC")]
        public static extern Int32 SearchCSC();

		[DllImport("Askcsc.dll", EntryPoint = "CSC_EHP_PARAMS_EXT")]
		public static extern Int32 CSC_EHP_PARAMS_EXT(Byte MaxNbCard, Byte Req, Byte NbSlot, Byte AFI, Byte AtoSelDiv, Byte Deselect, Byte SelectAppli, Byte Lg, Byte[] Data, Int16 FelicaAFI, Byte FelicaNBSlot); 

        [DllImport("Askcsc.dll", EntryPoint = "CSC_SearchCardExt")]
        public static extern Int32 SearchCardExt(ref sCARD_SearchExtTag _SearchExtender, Int32 searchMask, byte Forget, byte timeout, ref int Com,
                                                        ref Int32 lpcbAtr, byte[] lpATR);


        [DllImport("Askcsc.dll", EntryPoint = "CSC_SendReceive")]
        public static extern Int32 SendReceive(Int32 timeout, byte[] Commande, Int32 commandeLen, byte[] Response, ref  Int32 responseLen);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_SelectCID")]
        public static extern Int32 SelectCID(byte CID, ref byte[] status);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_TransparentCommandConfig")]
        public static extern Int32 TransparentCommandConfig(byte ISO, byte addCRC, byte chekCRC, byte field, byte[] ConfigIso, byte[] COnfigAddCRC,  byte[] COnbfigCheck, byte[] ConfigField);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_TransparentCommand")]
        public static extern Int32 TransparentCommand(byte[] command, Int32 len, byte[] status, ref Int32 lenResponse, byte[] Response);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_AddCRC")]
        public static extern Int32 AddCRC(byte[] buffer, ref Int32 len);

        [DllImport("Askcsc.dll", EntryPoint = "CSC_ISOCommand")]
        public static extern Int32 CSC_ISOCommand (byte[] command, Int32 len, byte[] BufOUT, ref Int32 lenResponse);


#endregion

        public struct sCARD_SearchExtTag
        {
            public byte  CONT;		// Contact Mode
            public byte  ISOB;		// ISO B Protocol Mode
            public byte  ISOA;		// ISO A Protocol Mode
            public byte  TICK;		// Ticket Mode
            public byte  INNO;		// Innovatron Protocol Mode
            public byte  MIFARE;	// Mifare Mode
            public byte  MV4k;		// MV4k protocol mode
            public byte  MV5k;		// MV5k protocol mode
            public byte  MONO;		// mono-search mode
        }


        #region Constantes
        // Définition des constantes
        public const int RCSC_Ok = 0x8001;
        public const int RCSC_OpenCOMError = 0x8002;
        public const int RCSC_NoAnswer = 0x8003;
        public const int RCSC_CheckSum = 0x8004;
        public const int RCSC_Fail = 0x8005;
        public const int RCSC_CardNotFound = 0x8006;
        public const int RCSC_AntennaFails = 0x8007;
        public const int RCSC_Timeout = 0x8008;
        public const int RCSC_DataWrong = 0x8009;
        public const int RCSC_Overflow = 0x800A;
        public const int RCSC_ErrorSAM = 0x800B;
        public const int RCSC_CSCNotFound = 0x800C;
        public const int RCSC_BadATR = 0x800D;
        public const int RCSC_TXError = 0x800E;
        public const int RCSC_WarningVersion = 0x800F;
        public const int RCSC_SelectSAMError = 0x8010;
        public const int RCSC_UnknownClassCommand = 0x8011;
        public const int RCSC_InputDataWrong = 0x8012;


        /* search masks for CSC_SearchCardExt function */

        public const int SEARCH_MASK_CONT = 0x0001;
        public const int SEARCH_MASK_ISOB = 0x0002;
        public const int SEARCH_MASK_ISOA = 0x0004;
        public const int SEARCH_MASK_TICK = 0x0008;
        public const int SEARCH_MASK_INNO = 0x0010;
        public const int SEARCH_MASK_MIFARE = 0x0020;
        public const int SEARCH_MASK_MV4K = 0x0040;
        public const int SEARCH_MASK_MV5K = 0x0080;
        public const int SEARCH_MASK_MONO = 0x0100;

        public const int SUCCESS = 0x01;	// functions returned value if success
        public const int FAILURE = 0x00;	// functions returned value if failure
        //public const byte[] VALUE_INIT1 = { 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x01, 0xFE, 0x01, 0xFE };


        public const int VERSIONLENGTH = 70;		// length of CSC-version
        public const int DATA_LENGTH = 16;	// length of data in card
        public const int ACTUAL_DATA_LG_VALUE = 4;	// length of actual data in value blocs
        public const int STATUS_OK = 0x00;	// MIFARE functions success returned value
        public const int FORG = 0x01;	// forget last serial number
        public const int TIMEOUT = 0x03;	// no time out
        public const int ERROR_VALUE_GET_PARAMS = 0xFF;	// value returned by
        #endregion

    }


    /// <summary>
    /// Classe qui permet de d Wrapper le lecteur RFID ASK
    /// </summary>
    public class RDReaderWrapperNET : IDisposable 
    {
        private Thread _ReadingThread; // Lecture sur le lecteur par un thread  
        private bool _isOnline = false; //Vérification de le a présence d'un lecteur RIFD
        private string _version = string.Empty; // Version du lecteur
        private bool _Iso15693 = false; // Mode d'utilistation du lecteur en iso15693
        private static bool isAlreadySearchReader = false;

        // Délegué utilisé lors de la lecture d'une carte 
        public  CSC.ReadCard onReadCard;

        public string Message
        {
          get { return _version; }
          
        }

        /// <summary>
        /// Determine si le lecteur est en ligne
        /// </summary>
        public bool IsOnline
        {
            get { return _isOnline; }
            private set { _isOnline = value; }
        }


        /// <summary>
        ///  Démarrage de lecture en thread 
        /// </summary>
        public void Start()
        {
            if (this.IsOnline && (this._ReadingThread == null || !this._ReadingThread.IsAlive))
            {
                this._ReadingThread = new Thread(new ThreadStart(this.ReadASK));
                this._ReadingThread.Priority = ThreadPriority.Lowest;
                this._ReadingThread.Start();
            }

        }

        /// <summary>
        /// Fin de lecture et libération des ressources 
        /// </summary>
        public void Stop()
        {
            if (this._ReadingThread != null && this._ReadingThread.IsAlive)
            {
                this._ReadingThread.Abort();
                this._ReadingThread.Join();
                this._ReadingThread = null;
                System.GC.Collect();
            }
        }


        /// <summary>
        /// Constructeur du Wrapper
        /// </summary>
        /// <param name="delegationRead">Méthode déléguée appellé si lecture</param>
        /// <param name="ISO15693">Mode d'utilisation Iso15693 si true</param>
        public RDReaderWrapperNET(CSC.ReadCard delegationRead,bool ISO15693)
        {

            /// Recherche d'un lecteur ASK
          
                if (CSC.SearchCSC() != CSC.RCSC_Ok)
                {
                    this._isOnline = false;
                    return;
                }
          
            try
            {
                
                if (delegationRead != null)
                    this.onReadCard = delegationRead;

                byte[] version = new byte[CSC.VERSIONLENGTH];
                CSC.VersionDLL(version);
                this._version = UnicodeEncoding.ASCII.GetString(version);

                
                this._isOnline = true;

                //Initilialisation de la méthode ISO15693
                if (ISO15693)
                {
                    IsOnline = false;
                    this._Iso15693 = true;
                    Int32 ret = -1;
                    Int32 lnRet = 0;
                   
                    
                    byte[] bufferOut = new byte[50];

                    #region  Initialisation du champON
                    byte[] bufferIN = { 0x80, 0x05, 0x01, 0x0E, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 };
                    Int32 len= bufferIN.Length -2;
                    if ((ret = CSC.AddCRC(bufferIN, ref  len)) == CSC.RCSC_Ok)
                    {
                        ret = CSC.SendReceive(2000, bufferIN, bufferIN.Length, bufferOut, ref lnRet);
                        if (ret != CSC.RCSC_Ok)
                            return;
                    }
                    else return;
                    #endregion
                    
                    #region  Initialisation du mode ISO15693
                    bufferIN = null;
                    bufferOut = null;
                    System.GC.Collect();

                    bufferIN =  new byte[]{ 0x80, 0x05, 0x10, 0x01, 0x02, 0x0C, 0x02, 0x00, 0x00, 0x00 };
                    bufferOut = new byte[50];
                    if ((ret = CSC.AddCRC(bufferIN, ref  len)) == CSC.RCSC_Ok)
                    {
                        ret = CSC.SendReceive(2000, bufferIN, bufferIN.Length, bufferOut, ref lnRet);
                        if (ret != CSC.RCSC_Ok)
                            return;
                    }
                    else return;
                    bufferIN = null;
                    bufferOut = null;
                    System.GC.Collect();
                    IsOnline = true;

                    #endregion


                }

            }
            catch (Exception ex)
            {
                this._version= ex.Message;
            }
           
        }


        #region IDisposable Membres

        /// <summary>
        /// Libération des resssources
        /// </summary>
        public void Dispose()
        {

            this.Stop();
        }

        #endregion

        /// <summary>
        /// Méthode read de lecture continuelle
        /// </summary>
        private void ReadASK()
        {
            while (true)
            {
                byte[] read;
                if (!_Iso15693) // Mode MyFare
                {

                    searchMifare(out read);
                    if (read != null && this.onReadCard != null)
                    {
                        this.onReadCard.Invoke(read);

                    }
                }
                else // Mode ISO15693
                {
                    if (SearISO15693(out read) && this.onReadCard != null)
                    {
                         this.onReadCard.Invoke(read);
                    }
                }

                Thread.Sleep(250);

            }
        }


        /// <summary>
        /// Recherche en mode Iso15693
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private bool SearISO15693(out byte[] read)
        {
            Int32 ret = -1;
            Int32 lnRet = 0;
            
            byte[] bufferIN;
            byte[] bufferOut;

            bufferIN = new byte[] { 0x80, 0x04, 0x15, 0x01, 0x26, 0x00, 0x00, 0x00, 0x00 };
            bufferOut = new byte[50];
            Int32 len = bufferIN.Length - 2;
            if ((ret = CSC.AddCRC(bufferIN, ref  len)) == CSC.RCSC_Ok)
            {
                ret = CSC.SendReceive(2000, bufferIN, bufferIN.Length, bufferOut, ref lnRet);
                if (ret == CSC.RCSC_Ok && lnRet > 9 )
                {
                    read = new byte[lnRet];
                    Array.Copy(bufferOut, read, lnRet-1);
                    return true;
                }

            }
            bufferIN = null;
            bufferOut = null;
            System.GC.Collect();
            IsOnline = true;
            read = new byte[0];
            return false;
        }


        /// <summary>
        /// Recherche en mode Mifare
        /// </summary>
        /// <param name="lecture"></param>
        /// <returns></returns>
        private Int32 searchMifare(out  byte[] lecture)
        /*****************************************************************************/
        {
            lecture = new byte[20];	
            CSC.sCARD_SearchExtTag cardSearch = new CSC.sCARD_SearchExtTag();				// config. structure of card research mode
            int com = 15693 ;						// comm mode found
            Int32 respLength = 0; 				// response length
            byte[] response = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0,0,0,0,0,0,0,0 };			// response of the card
            Int32 retAct;
            Int32 ret;
           

            cardSearch.ISOA = 2;
            cardSearch.ISOB = 2;
            cardSearch.TICK = 1;
            cardSearch.MV4k = 1;
            cardSearch.MV5k = 1;
            cardSearch.MIFARE=1;
            cardSearch.CONT = 1;
            cardSearch.MONO = 0;
            cardSearch.INNO = 1;


            retAct = CSC.SearchCardExt(ref cardSearch,
                            CSC.SEARCH_MASK_ISOA | CSC.SEARCH_MASK_ISOB | CSC.SEARCH_MASK_MIFARE | CSC.SEARCH_MASK_MV4K| CSC.SEARCH_MASK_INNO,
                            CSC.FORG,
                            CSC.TIMEOUT,
                            ref com,
                            ref respLength,
                            response); 						//activate Mifare


            Console.WriteLine(com.ToString());
            if ((retAct == CSC.RCSC_Ok) && (com == 0x08))
            {
                lecture = (byte[])response.Clone();

                ret = CSC.SUCCESS;
            }
            else
            {
                lecture = (byte[])response.Clone();
                ret = CSC.FAILURE;
            }
            return (ret);
            
        }

    }
}
