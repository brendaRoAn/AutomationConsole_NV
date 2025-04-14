using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RunTeamConsole.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using RunTeamConsole.Models.Packages;
using System.Data;
using System.Threading.Tasks;
using Maintenance = RunTeamConsole.Models.Maintenance;
using Process = RunTeamConsole.Models.Process;
using System.Diagnostics;
using RunTeamConsole.Models.DB2Install;
using System.Transactions;

namespace RunTeamConsole
{
    public class DataAccess
    {
        #region AsyncCode
        /*public static bool AsyncDataReady = false;

        public async void AsyncDataAccessStart()
        {
            AsyncDataReady = false;

            
            Debug.WriteLine("\n\n-- Request Catalog Task Started --\n\n");

            Task<List<Process>> TaskGetCat = AsyncGetCatalogInitConfig();
            List<Process> GetCatoutput = await TaskGetCat;

            Auxiliar.ProcessInitConfig = new List<Process>(GetCatoutput);

            foreach (Process p in Auxiliar.ProcessInitConfig)
            {
                p.StepList[p.StepList.Count - 1].AutoDefault = false;
            }
            
            Debug.WriteLine("\n\n-- Request Catalog Task Ended --\n\n");
                     
            //__________________________________________________________________

            Debug.WriteLine("\n\n-- Request List Task Started --\n\n");

            Task T1 = RequestList();
            await T1;

            //AsyncGetCloudServerSystems(); // Replaces Auxiliar.SalServerList.AddRange(GetCloudServerSystems());
            Task<List<ServerSystem>> T = AsyncGetCloudServerSystems();
            List<ServerSystem> cloudSystems = await T;
            Auxiliar.SalServerList.AddRange(cloudSystems);

            //AsyncGetCloudServerSystems(); // Replaces Auxiliar.SalServerList.AddRange(GetCloudServerSystems());
            Task<List<SaltMaster>> TM = AsyncGetMasterServers();
            List<SaltMaster> masterSystems = await TM;
            Auxiliar.SaltMastersList = new List<SaltMaster>(masterSystems);

            Debug.WriteLine("\n\n-- Request List Task Completed --\n\n");
            AsyncDataReady = true;
        }
        
        public static async Task<List<Process>> AsyncGetCatalogInitConfig()
        {
            string[] contentArray, tempArray;
            List<string> flowFiles = new List<string>();
            string catalogPath, team, title, subtype, osType, projectname, dbType, layoutFilename;
            bool applReq, secuential;
            catalogPath = Auxiliar.catalogPath + "CATALOG\\";
            List<Process> output = new List<Process>();
            Process TempProcess;
            OraclePackage TempOraclePackage;
            TransactionsPackage TempTransactionsPackage;
            SAPKernelPackage TempSAPKernelPackage;
            SapInstallCatalog TempSapInstallCatalog;
            Db2Install TempDb2InstallCatalog;
            //ScriptPackage TempScriptPackage;

            MessageBoxResult result;

            Auxiliar.SendLogRequest("Reading processes layout from path: " + catalogPath);

            await Task.Run(() => 
            {

                try
                {
                    contentArray = File.ReadAllLines(catalogPath + "CATALOG.CAT");
                }
                catch (Exception e)
                {
                    contentArray = new string[] { };
                    if (Auxiliar.catalogPath.Contains(".83") || Auxiliar.catalogPath.Contains("fitmedia"))
                    {
                        do
                        {
                            result = MessageBox.Show("Processes layout Reading... One Line of the CATALOG contains old reference...", "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                        } while (result != MessageBoxResult.OK);
                    }
                    else
                    {
                        int indexToCutMessage = e.Message.IndexOf(Auxiliar.catalogPath);
                        string message = e.Message.Substring(0, indexToCutMessage - 4);
                        //string workAround = "Try open \\\\10.130.19.40\\fitmedia_test on file explorer and log with SYNTAX domain credentials";
                        do
                        {
                            result = MessageBox.Show(message, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                        } while (result != MessageBoxResult.OK);
                    }
                    Auxiliar.SendLogRequest("Unable to read processes layout from path. " + e.Message);
                    Environment.Exit(0);
                }

                foreach (string line in contentArray)
                {
                    TempProcess = null;
                    tempArray = line.Split('|');
                    team = tempArray[0];
                    title = tempArray[1];
                    subtype = tempArray[2];
                    osType = tempArray[3];
                    projectname = tempArray[4];
                    dbType = tempArray[5];
                    applReq = Convert.ToBoolean(tempArray[6]);
                    secuential = Convert.ToBoolean(tempArray[7]);
                    layoutFilename = tempArray[8];

                    try
                    {
                        using (StreamReader r = new StreamReader(layoutFilename))
                        {
                            string json = r.ReadToEnd();
                            TempProcess = JsonConvert.DeserializeObject<Process>(json);
                            TempProcess.Team = team;
                            TempProcess.Title = title;
                            //TempProcess.Category = subtype;
                            //TempProcess.Name = projectname;
                            TempProcess.DBSType = dbType;
                            if (dbType == "UNIVERSAL")
                                TempProcess.PASOS = osType;
                            else
                                TempProcess.DBSOS = osType;
                            TempProcess.ApplReq = applReq;
                            TempProcess.IsSecuential = secuential;
                            foreach (Step step in TempProcess.StepList)
                            {
                                step.Process = TempProcess.ProjectName;
                            }
                            string packagesPath = Auxiliar.GetAppCatalogPath(TempProcess.ProjectName);
                            string catalogFileName = packagesPath + TempProcess.ProjectName + ".CATALOG";
                            if (File.Exists(catalogFileName))
                            {
                                var packagesContent = File.ReadAllLines(catalogFileName);

                                if (subtype == "DB2")
                                {
                                    if (TempProcess.TransactionsPackages == null)
                                        TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempSapInstallCatalog = null;
                                            tempArray = packagesContent[i].Split('|');
                                            string opSysType = tempArray[0].Trim();
                                            string osDist = tempArray[1].Trim();
                                            string osArch = tempArray[2].Trim();
                                            string sapProd = tempArray[3].Trim();
                                            string sapStack = tempArray[4].Trim();
                                            string sapKernel = tempArray[5].Trim();
                                            string dbName = tempArray[6].Trim();
                                            string dbVersion = tempArray[7].Trim();
                                            string dbPatch = tempArray[8].Trim();
                                            string osPatch = tempArray[9].Trim();
                                            string fileName = tempArray[10].Trim();
                                            string controlData = tempArray[11].Trim();
                                            string controlValue = tempArray[12].Trim();
                                            string fileDesc = tempArray[13].Trim();
                                            string control1 = tempArray[14].Trim();
                                            string control2 = tempArray[15].Trim();
                                            string control3 = tempArray[16].Trim();
                                            string control4 = tempArray[17].Trim();
                                            string control5 = tempArray[18].Trim();
                                            if (fileDesc == null)
                                                fileDesc = "";
                                            TempDb2InstallCatalog = TempProcess.Db2InstallCatalogs.Where(x => x.Db == dbName && x.OsType == opSysType && x.OsDistribution == osDist && x.OsArchitecture == osArch && x.DbVersion == dbVersion && x.DbPatch == dbPatch).FirstOrDefault();
                                            if (TempDb2InstallCatalog == null)
                                                TempProcess.Db2InstallCatalogs.Add(new Db2Install(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5));
                                            else
                                                TempDb2InstallCatalog.AddFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5);
                                        }
                                    }
                                }//Completed
                                else if (subtype == "INTERNAL_TEST")
                                {
                                    if (TempProcess.ProjectName.Contains("DB2UPDTFIXPACKLCLOUD"))
                                    {
                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if ((packagesContent[i].Length > 0) && !(packagesContent[i].Trim().StartsWith('=')))
                                            {
                                                TempSapInstallCatalog = null;
                                                tempArray = packagesContent[i].Split('|');
                                                string opSysType = tempArray[0].Trim();
                                                string osDist = tempArray[1].Trim();
                                                string osArch = tempArray[2].Trim();
                                                string sapProd = tempArray[3].Trim();
                                                string sapStack = tempArray[4].Trim();
                                                string sapKernel = tempArray[5].Trim();
                                                string dbName = tempArray[6].Trim();
                                                string dbVersion = tempArray[7].Trim();
                                                string dbPatch = tempArray[8].Trim();
                                                string osPatch = tempArray[9].Trim();
                                                string fileName = tempArray[10].Trim();
                                                string controlData = tempArray[11].Trim();
                                                string controlValue = tempArray[12].Trim();
                                                string fileDesc = tempArray[13].Trim();
                                                string control1 = tempArray[14].Trim();
                                                string control2 = tempArray[15].Trim();
                                                string control3 = tempArray[16].Trim();
                                                string control4 = tempArray[17].Trim();
                                                string control5 = tempArray[18].Trim();
                                                if (fileDesc == null)
                                                    fileDesc = "";
                                                TempDb2InstallCatalog = TempProcess.Db2InstallCatalogs.Where(x => x.Db == dbName && x.OsType == opSysType && x.OsDistribution == osDist && x.OsArchitecture == osArch && x.DbVersion == dbVersion && x.DbPatch == dbPatch).FirstOrDefault();
                                                if (TempDb2InstallCatalog == null)
                                                    TempProcess.Db2InstallCatalogs.Add(new Db2Install(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5));
                                                else
                                                    TempDb2InstallCatalog.AddFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5);
                                            }
                                        }
                                    }
                                    else if (TempProcess.ProjectName.Contains("SAPSYSTEMCOPY") || TempProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLPOSTACTIVITIES" || TempProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLPOST")
                                    {
                                        if (TempProcess.TransactionsPackages == null)
                                            TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                TempTransactionsPackage = null;
                                                tempArray = packagesContent[i].Split('|');
                                                string packageGroup = tempArray[0];
                                                string packageStep = "";
                                                string packageSubgroup = "";
                                                string defaultSelected = "";
                                                string TransactionCode = "";
                                                string TransactionDescription = "";
                                                
                                                packageStep = tempArray[2];
                                                packageSubgroup = tempArray[1];
                                                defaultSelected = tempArray[3];
                                                TransactionCode = tempArray[4];
                                                TransactionDescription = tempArray[5];
                                                TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                                TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                                tempTransaction.Step = packageStep;
                                                if (TempTransactionsPackage == null)
                                                {
                                                    List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                    transactionsList.Add(tempTransaction);
                                                    TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                    TempProcess.TransactionsPackages.Add(tempPkg);
                                                }
                                                else
                                                    TempTransactionsPackage.Transactions.Add(tempTransaction);
                                            }
                                        }

                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                TempTransactionsPackage = null;
                                                tempArray = packagesContent[i].Split('|');
                                                string packageGroup = tempArray[0];
                                                string packageStep = "";
                                                string packageSubgroup = "";
                                                string defaultSelected = "";
                                                string TransactionCode = "";
                                                string TransactionDescription = "";

                                                packageStep = tempArray[2];
                                                packageSubgroup = tempArray[1];
                                                defaultSelected = tempArray[3];
                                                TransactionCode = tempArray[4];
                                                TransactionDescription = tempArray[5];
                                                TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                                TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                                tempTransaction.Step = packageStep;
                                                if (TempTransactionsPackage == null)
                                                {
                                                    List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                    transactionsList.Add(tempTransaction);
                                                    TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                    TempProcess.TransactionsPackages.Add(tempPkg);
                                                }
                                                else
                                                    TempTransactionsPackage.Transactions.Add(tempTransaction);
                                            }
                                        }
                                    }
                                }//Completed
                                else if (subtype == "SAP_INSTALL")
                                {
                                    if (TempProcess.ProjectName.Equals("SAPINSTALLORACLECLOUD") || TempProcess.ProjectName.Equals("SAPINSTALLHANACLOUD") || TempProcess.ProjectName.Equals("SAPINSTALLAASCLOUD"))
                                    {
                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                TempSapInstallCatalog = null;
                                                tempArray = packagesContent[i].Split('|');
                                                if (!(tempArray[0].Trim().StartsWith('#')))
                                                {
                                                    string opSysType = tempArray[0].Trim();
                                                    string osDist = tempArray[1].Trim();
                                                    string osArch = tempArray[2].Trim();
                                                    string sapProd = tempArray[3].Trim();
                                                    string sapStack = tempArray[4].Trim();
                                                    string sapKernel = tempArray[5].Trim();
                                                    string dbName = tempArray[6].Trim();
                                                    string dbVersion = tempArray[7].Trim();
                                                    string dbPatch = tempArray[8].Trim();
                                                    string osPatch = tempArray[9].Trim();
                                                    string fileName = tempArray[10].Trim();
                                                    string controlData = tempArray[11].Trim();
                                                    string controlValue = tempArray[12].Trim();
                                                    string fileDesc = tempArray[13].Trim();
                                                    string control1 = tempArray[14].Trim();
                                                    string control2 = tempArray[15].Trim();
                                                    string control3 = tempArray[16].Trim();
                                                    string control4 = tempArray[17].Trim();
                                                    string control5 = tempArray[18].Trim();
                                                    if (fileDesc == null)
                                                        fileDesc = "";
                                                    TempSapInstallCatalog = TempProcess.SapInstallCatalogs.Where(x => x.OpSysType == opSysType && x.OsDist == osDist && x.OsArch == osArch && x.SapProd == sapProd && x.SapStack == sapStack && x.SapKernel == sapKernel && x.DbName == dbName && x.DbVersion == dbVersion && x.DbPatch == dbPatch).FirstOrDefault();
                                                    if (TempSapInstallCatalog == null)
                                                        TempProcess.SapInstallCatalogs.Add(new SapInstallCatalog(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5));
                                                    else
                                                        TempSapInstallCatalog.AddFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5);
                                                }
                                            }
                                        }
                                    }
                                }//Completed
                                else if (subtype == "SAP" || subtype == "SAP_OLD")
                                {
                                    if (TempProcess.ProjectName.Contains("JAVA"))
                                    {
                                        for (int i = 1; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                TempProcess.JavaComponents.Add(new JavaComponent(packagesContent[i]));
                                            }
                                        }
                                    }
                                    else if (TempProcess.ProjectName.Contains("SAPHOSTAGENTUPGRADE"))
                                    {
                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                tempArray = packagesContent[i].Split('|');
                                                string osTypeSAP = tempArray[0];
                                                string version = tempArray[1];
                                                string patch = tempArray[2];
                                                //string db = tempArray[3];
                                                string packageFileName = tempArray[4];
                                                string packageFilePath = tempArray[5];
                                                string os = tempArray[6];
                                                string packageFileMode = tempArray[7];
                                                if (packageFileMode == null)
                                                    packageFileMode = "";
                                                TempProcess.SAPHostAgentPackages.Add(new SAPHostAgentPackage(osTypeSAP, version, patch, packageFileName, packageFilePath, os, packageFileMode));
                                            }
                                        }
                                    }
                                    else if (TempProcess.ProjectName.Contains("SAPKERNELUPGRADE"))
                                    {
                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                TempSAPKernelPackage = null;
                                                tempArray = packagesContent[i].Split('|');
                                                string unicode = tempArray[0];
                                                string version = tempArray[1];
                                                string patch = tempArray[2];
                                                string db = tempArray[3];
                                                string packageFileName = tempArray[4];
                                                string packageFilePath = tempArray[5];
                                                string os = tempArray[6];
                                                string packageFileMode = "";
                                                //string packageFileMode = tempArray[7];
                                                if (tempArray.Length > 7)
                                                {
                                                    if (tempArray[7] != null)
                                                        packageFileMode = tempArray[7];
                                                }
                                                /*if (packageFileMode == null)
                                                    packageFileMode = "";
                                                TempSAPKernelPackage = TempProcess.SAPKernelPackages.Where(x => x.Unicode == unicode && x.Version == version && x.Patch == patch && x.DB == db).FirstOrDefault();
                                                if (TempSAPKernelPackage == null)
                                                    TempProcess.SAPKernelPackages.Add(new SAPKernelPackage(unicode, version, patch, db, packageFileName, packageFilePath, os, packageFileMode));
                                                else
                                                    TempSAPKernelPackage.AddFile(packageFileName, packageFilePath, os, packageFileMode);
                                            }
                                        }
                                    }
                                    else if (TempProcess.ProjectName.Contains("SAPSYSTEMCOPY") || TempProcess.ProjectName.Trim().ToUpper() == "SAPQC" || TempProcess.ProjectName.Trim().ToUpper() == "SAPQCNGZTCLOUD")
                                    {
                                        if (TempProcess.TransactionsPackages == null)
                                            TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                TempTransactionsPackage = null;
                                                tempArray = packagesContent[i].Split('|');
                                                string packageGroup = tempArray[0];
                                                string packageStep = "";
                                                string packageSubgroup = "";
                                                string defaultSelected = "";
                                                string TransactionCode = "";
                                                string TransactionDescription = "";
                                                
                                                packageStep = tempArray[2];
                                                packageSubgroup = tempArray[1];
                                                defaultSelected = tempArray[3];
                                                TransactionCode = tempArray[4];
                                                TransactionDescription = tempArray[5];
                                                TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                                TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                                tempTransaction.Step = packageStep;
                                                if (TempTransactionsPackage == null)
                                                {
                                                    List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                    transactionsList.Add(tempTransaction);
                                                    TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                    TempProcess.TransactionsPackages.Add(tempPkg);
                                                }
                                                else
                                                    TempTransactionsPackage.Transactions.Add(tempTransaction);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (TempProcess.TransactionsPackages == null)
                                            TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                        for (int i = 0; i < packagesContent.Length; i++)
                                        {
                                            if (packagesContent[i].Length > 0)
                                            {
                                                TempTransactionsPackage = null;
                                                tempArray = packagesContent[i].Split('|');
                                                string packageGroup = tempArray[0];
                                                string packageStep = "";
                                                string packageSubgroup = "";
                                                string defaultSelected = "";
                                                string TransactionCode = "";
                                                string TransactionDescription = "";
                                                if (TempProcess.ProjectName.Contains("SAPSYSTEMCOPY"))
                                                {
                                                    packageStep = tempArray[2];
                                                    packageSubgroup = tempArray[1];
                                                    defaultSelected = tempArray[3];
                                                    TransactionCode = tempArray[4];
                                                    TransactionDescription = tempArray[5];
                                                    TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                                    TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                                    tempTransaction.Step = packageStep;
                                                    if (TempTransactionsPackage == null)
                                                    {
                                                        List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                        transactionsList.Add(tempTransaction);
                                                        TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                        TempProcess.TransactionsPackages.Add(tempPkg);
                                                    }
                                                    else
                                                        TempTransactionsPackage.Transactions.Add(tempTransaction);
                                                }
                                                else
                                                {
                                                    packageSubgroup = tempArray[1];
                                                    defaultSelected = tempArray[2];
                                                    TransactionCode = tempArray[3];
                                                    TransactionDescription = tempArray[4];
                                                    TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                                    if (TempTransactionsPackage == null)
                                                        TempProcess.TransactionsPackages.Add(new TransactionsPackage(packageGroup, packageSubgroup, defaultSelected, TransactionCode, TransactionDescription));
                                                    else
                                                        TempTransactionsPackage.AddTransaction(defaultSelected, TransactionCode, TransactionDescription);
                                                }
                                            }
                                        }
                                    }
                                }//Completed
                                else if (subtype == "ORACLE" || subtype == "ORACLE_OLD")
                                {
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempOraclePackage = null;
                                            string[] lineArray;
                                            lineArray = packagesContent[i].Split('|');
                                            string packageOS = lineArray[0];
                                            string pkgDBVersion = lineArray[1];
                                            string packageName = lineArray[2];
                                            string packageShortName = lineArray[3];
                                            string packageFileName = lineArray[4];
                                            string packageFileDescription = lineArray[5];
                                            string packageFilePath = lineArray[6];
                                            string packageFileMode = ""; //Variable relacionada al ajuste de los catalogos de paqueteria
                                            int lineLen = lineArray.Length;
                                            //MessageBox.Show(lineLen.ToString());
                                            if (lineLen > 7)
                                            {
                                                if (lineArray[7] != null)
                                                    packageFileMode = lineArray[7];
                                            }
                                            TempOraclePackage = TempProcess.OraclePackages.Where(x => x.Name == packageName && x.OS == packageOS && x.DBVersion == pkgDBVersion).FirstOrDefault();
                                            if (TempOraclePackage == null)
                                                TempProcess.OraclePackages.Add(new OraclePackage(packageName, packageOS, pkgDBVersion, packageShortName, packageFileName, packageFileDescription, packageFilePath, packageFileMode));
                                            else
                                                TempOraclePackage.AddFile(packageShortName, packageFileName, packageFileDescription, packageFilePath, packageFileMode);
                                        }
                                    }
                                }//Completed
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("Could not find file"))
                            Auxiliar.SendLogRequest("Could not find file" + layoutFilename);
                        else
                            Auxiliar.SendLogRequest(layoutFilename + " is corrupted");
                    }

                    if (TempProcess != null)
                    {
                        output.Add(TempProcess);
                    }
                }

            });

            return output;
        }
        
        public static async Task RequestList()
        {
            List<ServerSystem> systems = new List<ServerSystem>();
            string salServersFile = Auxiliar.catalogPath + "CATALOG\\SALSERVERS.JSON";

            await Task.Run(async () =>
            {

                if (File.Exists(salServersFile))
                {
                    FileInfo fi = new FileInfo(salServersFile);
                    if (DateTime.Now < (fi.LastWriteTime.AddHours(12)))
                    {
                        using (StreamReader r = new StreamReader(salServersFile))
                        {
                            string json = r.ReadToEnd();
                            systems = JsonConvert.DeserializeObject<List<ServerSystem>>(json);
                        }
                    }
                    else
                    {
                        Task<List<ServerSystem>> T = AsyncRequestSalServersAndSave(salServersFile);
                        systems = await T;

                        if (systems.Count == 0)
                        {
                            using (StreamReader r = new StreamReader(salServersFile))
                            {
                                try
                                {
                                    string json = r.ReadToEnd();
                                    systems = JsonConvert.DeserializeObject<List<ServerSystem>>(json);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error reading json file");
                                }
                            }
                        }
                    }
                }
                else
                {
                    Task<List<ServerSystem>> T = AsyncRequestSalServersAndSave(salServersFile);
                    systems = await T;
                }

                if (systems.Count == 0)
                {
                    MessageBoxResult result;
                    do
                    {
                        result = MessageBox.Show("Unable to get servers or write servers on file. " + salServersFile, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                    } while (result != MessageBoxResult.OK);

                    Environment.Exit(0);
                }

            });

            Auxiliar.SalServerList = systems;
        }

        public static async Task<List<ServerSystem>> AsyncRequestSalServersAndSave(string salServersFile)
        {
            List<ServerSystem> systems = new List<ServerSystem>();
            string salURL, jsonString;
            salURL = "http://sal.us.fit/sal/server/data?sEcho=1&iColumns=13&sColumns=&iDisplayStart=0&iDisplayLength=-1";

            await Task.Run(() => {

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        var json = wc.DownloadString(salURL);
                        var details = JObject.Parse(json);
                        var aaData = details.Last;
                        foreach (var server in aaData.First)
                        {
                            systems.Add(new ServerSystem(server));
                        }
                        jsonString = JsonConvert.SerializeObject(systems);
                        File.WriteAllText(salServersFile, jsonString);
                    }
                }
                catch (Exception e)
                {
                    Auxiliar.SendLogRequest("Unable to get servers or write servers from file. " + e.Message);
                }

            });
            
            return systems;
        }

        public static async Task<List<ServerSystem>> AsyncGetCloudServerSystems()
        {
            List<ServerSystem> servers = new List<ServerSystem>();
            string cloudServersFile = Auxiliar.catalogPath + "CATALOG\\SERVERSLIST_CLOUD.JSON";

            await Task.Run(() => 
            {

                if (File.Exists(cloudServersFile))
                {
                    using (StreamReader r = new StreamReader(cloudServersFile))
                    {
                        try
                        {
                            string json = r.ReadToEnd();
                            servers = JsonConvert.DeserializeObject<List<ServerSystem>>(json);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error reading json file");
                        }
                    }
                }

            });
                
            return servers;
        }

        public static async Task<List<SaltMaster>> AsyncGetMasterServers()
        {
            List<SaltMaster> masters = new List<SaltMaster>();
            string saltServersCatalogFile = Auxiliar.catalogPath + "CATALOG\\MASTERSALTCATALOG.CAT";
            await Task.Run(() => 
            {

                if (File.Exists(saltServersCatalogFile))
                {
                    string[] content = File.ReadAllLines(saltServersCatalogFile);
                    foreach (string line in content)
                    {
                        string[] tempData = line.Split("|");
                        SaltMaster master = new SaltMaster(tempData[0], tempData[1], tempData[2], tempData[3], tempData[4], tempData[5]);
                        masters.Add(master);
                    }
                }

            });
            return masters;
        }*/
        #endregion

        const int messagePossition = 14;

        public DataAccess()
        {
            Auxiliar.Certificates = new List<Certificate>(Auxiliar.GetAllUserCertificates());
            Auxiliar.Authorizations = new List<Models.Authorization>(Auxiliar.GetAllUserAuthorizations());
            if (Auxiliar.Certificates.Any(x => x.ProcessName == "AUTOMATIONCONSOLE") && Auxiliar.Authorizations.Any(x => x.ProcessName == "AUTOMATIONCONSOLE"))
            {
                //AsyncDataAccessStart();
                Auxiliar.ProcessInitConfig = new List<Process>(GetCatalogInitConfig());

                foreach (Process p in Auxiliar.ProcessInitConfig)
                {
                    p.StepList[p.StepList.Count - 1].AutoDefault = false;
                }
                Auxiliar.SalServerList = new List<ServerSystem>(GetServerSystems());
                Auxiliar.SalServerList.AddRange(GetCloudServerSystems());
                Auxiliar.SaltMastersList = new List<SaltMaster>(GetMasterServers());
            }
            else
            {
                Auxiliar.SendLogRequest("You don't have the propper permissions to operate this tool. ");

                MessageBoxResult result;
                do
                {
                    result = MessageBox.Show("You don't have the propper permissions to operate this tool", Auxiliar.appTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                } while (result != MessageBoxResult.OK);

                Environment.Exit(0);
            }

        }

        public List<Process> GetCatalogInitConfig()
        {
            string[] contentArray, tempArray;
            List<string> flowFiles = new List<string>();
            string catalogPath, team, title, subtype, osType, projectname, dbType, layoutFilename;
            bool applReq, secuential;
            catalogPath = Auxiliar.catalogPath + "CATALOG\\";
            List<Process> output = new List<Process>();
            Process TempProcess;
            OraclePackage TempOraclePackage;
            TransactionsPackage TempTransactionsPackage;
            SAPKernelPackage TempSAPKernelPackage;
            SapInstallCatalog TempSapInstallCatalog;
            Db2Install TempDb2InstallCatalog;

            MessageBoxResult result;

            Auxiliar.SendLogRequest("Reading processes layout from path: " + catalogPath);
            try
            {
                contentArray = File.ReadAllLines(catalogPath + "CATALOG.CAT");
            }
            catch (Exception e)
            {
                contentArray = new string[] { };
                if (Auxiliar.catalogPath.Contains(".83") || Auxiliar.catalogPath.Contains("fitmedia"))
                {
                    do
                    {
                        result = MessageBox.Show("Processes layout Reading... One Line of the CATALOG contains old reference...", "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                    } while (result != MessageBoxResult.OK);
                }
                else
                {
                    int indexToCutMessage = e.Message.IndexOf(Auxiliar.catalogPath);
                    string message = e.Message.Substring(0, indexToCutMessage - 4);
                    //string workAround = "Try open \\\\10.130.19.40\\fitmedia_test on file explorer and log with SYNTAX domain credentials";
                    do
                    {
                        result = MessageBox.Show(message, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                    } while (result != MessageBoxResult.OK);
                }
                Auxiliar.SendLogRequest("Unable to read processes layout from path. " + e.Message);
                Environment.Exit(0);
            }

            foreach (string line in contentArray)
            {
                TempProcess = null;
                tempArray = line.Split('|');
                team = tempArray[0];
                title = tempArray[1];
                subtype = tempArray[2];
                osType = tempArray[3];
                projectname = tempArray[4];
                dbType = tempArray[5];
                applReq = Convert.ToBoolean(tempArray[6]);
                secuential = Convert.ToBoolean(tempArray[7]);
                layoutFilename = tempArray[8];

                try
                {
                    using (StreamReader r = new StreamReader(layoutFilename))
                    {
                        string json = r.ReadToEnd();
                        TempProcess = JsonConvert.DeserializeObject<Process>(json);
                        TempProcess.Team = team;
                        TempProcess.Title = title;
                        //TempProcess.Category = subtype;
                        //TempProcess.Name = projectname;
                        TempProcess.DBSType = dbType;
                        if (dbType == "UNIVERSAL")
                            TempProcess.PASOS = osType;
                        else
                            TempProcess.DBSOS = osType;
                        TempProcess.ApplReq = applReq;
                        TempProcess.IsSecuential = secuential;
                        foreach (Step step in TempProcess.StepList)
                        {
                            step.Process = TempProcess.ProjectName;
                        }
                        string packagesPath = Auxiliar.GetAppCatalogPath(TempProcess.ProjectName);
                        string catalogFileName = packagesPath + TempProcess.ProjectName + ".CATALOG";
                        if (File.Exists(catalogFileName))
                        {
                            var packagesContent = File.ReadAllLines(catalogFileName);

                            if (subtype == "DB2")
                            {
                                if (TempProcess.TransactionsPackages == null)
                                    TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                for (int i = 0; i < packagesContent.Length; i++)
                                {
                                    if (packagesContent[i].Length > 0)
                                    {
                                        TempSapInstallCatalog = null;
                                        tempArray = packagesContent[i].Split('|');
                                        string opSysType = tempArray[0].Trim();
                                        string osDist = tempArray[1].Trim();
                                        string osArch = tempArray[2].Trim();
                                        string sapProd = tempArray[3].Trim();
                                        string sapStack = tempArray[4].Trim();
                                        string sapKernel = tempArray[5].Trim();
                                        string dbName = tempArray[6].Trim();
                                        string dbVersion = tempArray[7].Trim();
                                        string dbPatch = tempArray[8].Trim();
                                        string osPatch = tempArray[9].Trim();
                                        string fileName = tempArray[10].Trim();
                                        string controlData = tempArray[11].Trim();
                                        string controlValue = tempArray[12].Trim();
                                        string fileDesc = tempArray[13].Trim();
                                        string control1 = tempArray[14].Trim();
                                        string control2 = tempArray[15].Trim();
                                        string control3 = tempArray[16].Trim();
                                        string control4 = tempArray[17].Trim();
                                        string control5 = tempArray[18].Trim();
                                        if (fileDesc == null)
                                            fileDesc = "";
                                        TempDb2InstallCatalog = TempProcess.Db2InstallCatalogs.Where(x => x.Db == dbName && x.OsType == opSysType && x.OsDistribution == osDist && x.OsArchitecture == osArch && x.DbVersion == dbVersion && x.DbPatch == dbPatch).FirstOrDefault();
                                        if (TempDb2InstallCatalog == null)
                                            TempProcess.Db2InstallCatalogs.Add(new Db2Install(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5));
                                        else
                                            TempDb2InstallCatalog.AddFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5);
                                    }
                                }
                            }//Completed
                            else if (subtype == "INTERNAL_TEST")
                            {
                                if (TempProcess.ProjectName.Contains("DB2UPDTFIXPACKLCLOUD"))
                                {
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if ((packagesContent[i].Length > 0) && !(packagesContent[i].Trim().StartsWith('=')))
                                        {
                                            TempSapInstallCatalog = null;
                                            tempArray = packagesContent[i].Split('|');
                                            string opSysType = tempArray[0].Trim();
                                            string osDist = tempArray[1].Trim();
                                            string osArch = tempArray[2].Trim();
                                            string sapProd = tempArray[3].Trim();
                                            string sapStack = tempArray[4].Trim();
                                            string sapKernel = tempArray[5].Trim();
                                            string dbName = tempArray[6].Trim();
                                            string dbVersion = tempArray[7].Trim();
                                            string dbPatch = tempArray[8].Trim();
                                            string osPatch = tempArray[9].Trim();
                                            string fileName = tempArray[10].Trim();
                                            string controlData = tempArray[11].Trim();
                                            string controlValue = tempArray[12].Trim();
                                            string fileDesc = tempArray[13].Trim();
                                            string control1 = tempArray[14].Trim();
                                            string control2 = tempArray[15].Trim();
                                            string control3 = tempArray[16].Trim();
                                            string control4 = tempArray[17].Trim();
                                            string control5 = tempArray[18].Trim();
                                            if (fileDesc == null)
                                                fileDesc = "";
                                            TempDb2InstallCatalog = TempProcess.Db2InstallCatalogs.Where(x => x.Db == dbName && x.OsType == opSysType && x.OsDistribution == osDist && x.OsArchitecture == osArch && x.DbVersion == dbVersion && x.DbPatch == dbPatch).FirstOrDefault();
                                            if (TempDb2InstallCatalog == null)
                                                TempProcess.Db2InstallCatalogs.Add(new Db2Install(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5));
                                            else
                                                TempDb2InstallCatalog.AddFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5);
                                        }
                                    }
                                }
                                else if (TempProcess.ProjectName.Contains("SAPSYSTEMCOPY") || TempProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLPOSTACTIVITIES" || TempProcess.ProjectName.Trim().ToUpper() == "SAPINSTALLPOST")
                                {
                                    if (TempProcess.TransactionsPackages == null)
                                        TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempTransactionsPackage = null;
                                            tempArray = packagesContent[i].Split('|');
                                            string packageGroup = tempArray[0];
                                            string packageStep = "";
                                            string packageSubgroup = "";
                                            string defaultSelected = "";
                                            string TransactionCode = "";
                                            string TransactionDescription = "";

                                            packageStep = tempArray[2];
                                            packageSubgroup = tempArray[1];
                                            defaultSelected = tempArray[3];
                                            TransactionCode = tempArray[4];
                                            TransactionDescription = tempArray[5];
                                            TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                            TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                            tempTransaction.Step = packageStep;
                                            if (TempTransactionsPackage == null)
                                            {
                                                List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                transactionsList.Add(tempTransaction);
                                                TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                TempProcess.TransactionsPackages.Add(tempPkg);
                                            }
                                            else
                                                TempTransactionsPackage.Transactions.Add(tempTransaction);
                                        }
                                    }

                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempTransactionsPackage = null;
                                            tempArray = packagesContent[i].Split('|');
                                            string packageGroup = tempArray[0];
                                            string packageStep = "";
                                            string packageSubgroup = "";
                                            string defaultSelected = "";
                                            string TransactionCode = "";
                                            string TransactionDescription = "";

                                            packageStep = tempArray[2];
                                            packageSubgroup = tempArray[1];
                                            defaultSelected = tempArray[3];
                                            TransactionCode = tempArray[4];
                                            TransactionDescription = tempArray[5];
                                            TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                            TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                            tempTransaction.Step = packageStep;
                                            if (TempTransactionsPackage == null)
                                            {
                                                List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                transactionsList.Add(tempTransaction);
                                                TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                TempProcess.TransactionsPackages.Add(tempPkg);
                                            }
                                            else
                                                TempTransactionsPackage.Transactions.Add(tempTransaction);
                                        }
                                    }
                                }
                            }//Completed
                            else if (subtype == "SAP_INSTALL")
                            {
                                if (TempProcess.ProjectName.Equals("SAPINSTALLORACLECLOUD") || TempProcess.ProjectName.Equals("SAPINSTALLHANACLOUD") || TempProcess.ProjectName.Equals("SAPINSTALLAASCLOUD"))
                                {
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempSapInstallCatalog = null;
                                            tempArray = packagesContent[i].Split('|');
                                            if (!(tempArray[0].Trim().StartsWith('#')))
                                            {
                                                string opSysType = tempArray[0].Trim();
                                                string osDist = tempArray[1].Trim();
                                                string osArch = tempArray[2].Trim();
                                                string sapProd = tempArray[3].Trim();
                                                string sapStack = tempArray[4].Trim();
                                                string sapKernel = tempArray[5].Trim();
                                                string dbName = tempArray[6].Trim();
                                                string dbVersion = tempArray[7].Trim();
                                                string dbPatch = tempArray[8].Trim();
                                                string osPatch = tempArray[9].Trim();
                                                string fileName = tempArray[10].Trim();
                                                string controlData = tempArray[11].Trim();
                                                string controlValue = tempArray[12].Trim();
                                                string fileDesc = tempArray[13].Trim();
                                                string control1 = tempArray[14].Trim();
                                                string control2 = tempArray[15].Trim();
                                                string control3 = tempArray[16].Trim();
                                                string control4 = tempArray[17].Trim();
                                                string control5 = tempArray[18].Trim();
                                                if (fileDesc == null)
                                                    fileDesc = "";
                                                TempSapInstallCatalog = TempProcess.SapInstallCatalogs.Where(x => x.OpSysType == opSysType && x.OsDist == osDist && x.OsArch == osArch && x.SapProd == sapProd && x.SapStack == sapStack && x.SapKernel == sapKernel && x.DbName == dbName && x.DbVersion == dbVersion && x.DbPatch == dbPatch).FirstOrDefault();
                                                if (TempSapInstallCatalog == null)
                                                    TempProcess.SapInstallCatalogs.Add(new SapInstallCatalog(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5));
                                                else
                                                    TempSapInstallCatalog.AddFile(opSysType, osDist, osArch, sapProd, sapStack, sapKernel, dbName, dbVersion, dbPatch, osPatch, fileName, controlData, controlValue, fileDesc, control1, control2, control3, control4, control5);
                                            }
                                        }
                                    }
                                }
                            }//Completed
                            else if (subtype == "SAP" || subtype == "SAP_OLD")
                            {
                                if (TempProcess.ProjectName.Contains("JAVA"))
                                {
                                    for (int i = 1; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempProcess.JavaComponents.Add(new JavaComponent(packagesContent[i]));
                                        }
                                    }
                                }
                                else if (TempProcess.ProjectName.Contains("SAPHOSTAGENTUPGRADE"))
                                {
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            tempArray = packagesContent[i].Split('|');
                                            string osTypeSAP = tempArray[0];
                                            string version = tempArray[1];
                                            string patch = tempArray[2];
                                            //string db = tempArray[3];
                                            string packageFileName = tempArray[4];
                                            string packageFilePath = tempArray[5];
                                            string os = tempArray[6];
                                            string packageFileMode = tempArray[7];
                                            if (packageFileMode == null)
                                                packageFileMode = "";
                                            TempProcess.SAPHostAgentPackages.Add(new SAPHostAgentPackage(osTypeSAP, version, patch, packageFileName, packageFilePath, os, packageFileMode));
                                        }
                                    }
                                }
                                else if (TempProcess.ProjectName.Contains("SAPKERNELUPGRADE"))
                                {
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempSAPKernelPackage = null;
                                            tempArray = packagesContent[i].Split('|');
                                            string unicode = tempArray[0];
                                            string version = tempArray[1];
                                            string patch = tempArray[2];
                                            string db = tempArray[3];
                                            string packageFileName = tempArray[4];
                                            string packageFilePath = tempArray[5];
                                            string os = tempArray[6];
                                            string packageFileMode = "";
                                            //string packageFileMode = tempArray[7];
                                            if (tempArray.Length > 7)
                                            {
                                                if (tempArray[7] != null)
                                                    packageFileMode = tempArray[7];
                                            }
                                            /*if (packageFileMode == null)
                                                packageFileMode = "";*/
                                            TempSAPKernelPackage = TempProcess.SAPKernelPackages.Where(x => x.Unicode == unicode && x.Version == version && x.Patch == patch && x.DB == db).FirstOrDefault();
                                            if (TempSAPKernelPackage == null)
                                                TempProcess.SAPKernelPackages.Add(new SAPKernelPackage(unicode, version, patch, db, packageFileName, packageFilePath, os, packageFileMode));
                                            else
                                                TempSAPKernelPackage.AddFile(packageFileName, packageFilePath, os, packageFileMode);
                                        }
                                    }
                                }
                                else if (TempProcess.ProjectName.Contains("SAPSYSTEMCOPY") || TempProcess.ProjectName.Trim().ToUpper() == "SAPQC" || TempProcess.ProjectName.Trim().ToUpper() == "SAPQCNGZTCLOUD")
                                {
                                    if (TempProcess.TransactionsPackages == null)
                                        TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempTransactionsPackage = null;
                                            tempArray = packagesContent[i].Split('|');
                                            string packageGroup = tempArray[0];
                                            string packageStep = "";
                                            string packageSubgroup = "";
                                            string defaultSelected = "";
                                            string TransactionCode = "";
                                            string TransactionDescription = "";

                                            packageStep = tempArray[2];
                                            packageSubgroup = tempArray[1];
                                            defaultSelected = tempArray[3];
                                            TransactionCode = tempArray[4];
                                            TransactionDescription = tempArray[5];
                                            TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                            TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                            tempTransaction.Step = packageStep;
                                            if (TempTransactionsPackage == null)
                                            {
                                                List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                transactionsList.Add(tempTransaction);
                                                TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                TempProcess.TransactionsPackages.Add(tempPkg);
                                            }
                                            else
                                                TempTransactionsPackage.Transactions.Add(tempTransaction);
                                        }
                                    }
                                }
                                else
                                {
                                    if (TempProcess.TransactionsPackages == null)
                                        TempProcess.TransactionsPackages = new List<TransactionsPackage>();
                                    for (int i = 0; i < packagesContent.Length; i++)
                                    {
                                        if (packagesContent[i].Length > 0)
                                        {
                                            TempTransactionsPackage = null;
                                            tempArray = packagesContent[i].Split('|');
                                            string packageGroup = tempArray[0];
                                            string packageStep = "";
                                            string packageSubgroup = "";
                                            string defaultSelected = "";
                                            string TransactionCode = "";
                                            string TransactionDescription = "";
                                            if (TempProcess.ProjectName.Contains("SAPSYSTEMCOPY"))
                                            {
                                                packageStep = tempArray[2];
                                                packageSubgroup = tempArray[1];
                                                defaultSelected = tempArray[3];
                                                TransactionCode = tempArray[4];
                                                TransactionDescription = tempArray[5];
                                                TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                                TransactionsPackage.Transaction tempTransaction = new TransactionsPackage.Transaction(defaultSelected, TransactionCode, TransactionDescription);
                                                tempTransaction.Step = packageStep;
                                                if (TempTransactionsPackage == null)
                                                {
                                                    List<TransactionsPackage.Transaction> transactionsList = new List<TransactionsPackage.Transaction>();
                                                    transactionsList.Add(tempTransaction);
                                                    TransactionsPackage tempPkg = new TransactionsPackage(packageGroup, packageSubgroup, transactionsList);
                                                    TempProcess.TransactionsPackages.Add(tempPkg);
                                                }
                                                else
                                                    TempTransactionsPackage.Transactions.Add(tempTransaction);
                                            }
                                            else
                                            {
                                                packageSubgroup = tempArray[1];
                                                defaultSelected = tempArray[2];
                                                TransactionCode = tempArray[3];
                                                TransactionDescription = tempArray[4];
                                                TempTransactionsPackage = TempProcess.TransactionsPackages.Where(x => x.Subgroup == packageSubgroup).FirstOrDefault();
                                                if (TempTransactionsPackage == null)
                                                    TempProcess.TransactionsPackages.Add(new TransactionsPackage(packageGroup, packageSubgroup, defaultSelected, TransactionCode, TransactionDescription));
                                                else
                                                    TempTransactionsPackage.AddTransaction(defaultSelected, TransactionCode, TransactionDescription);
                                            }
                                        }
                                    }
                                }
                            }//Completed
                            else if (subtype == "ORACLE" || subtype == "ORACLE_OLD")
                            {
                                for (int i = 0; i < packagesContent.Length; i++)
                                {
                                    if (packagesContent[i].Length > 0)
                                    {
                                        TempOraclePackage = null;
                                        string[] lineArray;
                                        lineArray = packagesContent[i].Split('|');
                                        string packageOS = lineArray[0];
                                        string pkgDBVersion = lineArray[1];
                                        string packageName = lineArray[2];
                                        string packageShortName = lineArray[3];
                                        string packageFileName = lineArray[4];
                                        string packageFileDescription = lineArray[5];
                                        string packageFilePath = lineArray[6];
                                        string packageFileMode = ""; //Variable relacionada al ajuste de los catalogos de paqueteria
                                        int lineLen = lineArray.Length;
                                        //MessageBox.Show(lineLen.ToString());
                                        if (lineLen > 7)
                                        {
                                            if (lineArray[7] != null)
                                                packageFileMode = lineArray[7];
                                        }
                                        TempOraclePackage = TempProcess.OraclePackages.Where(x => x.Name == packageName && x.OS == packageOS && x.DBVersion == pkgDBVersion).FirstOrDefault();
                                        if (TempOraclePackage == null)
                                            TempProcess.OraclePackages.Add(new OraclePackage(packageName, packageOS, pkgDBVersion, packageShortName, packageFileName, packageFileDescription, packageFilePath, packageFileMode));
                                        else
                                            TempOraclePackage.AddFile(packageShortName, packageFileName, packageFileDescription, packageFilePath, packageFileMode);
                                    }
                                }
                            }//Completed
                        }
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Could not find file"))
                        Auxiliar.SendLogRequest("Could not find file" + layoutFilename);
                    else
                        Auxiliar.SendLogRequest(layoutFilename + " is corrupted");
                }

                if (TempProcess != null)
                {
                    output.Add(TempProcess);
                }
            }

            return output;
        }

        public static void GetExportTablesByCustomer(string customerName, string currentProjectName, Process originalTempProcess)
        {
            Component TempComponent;
            string[] tempArray;
            string packagesPath = Auxiliar.GetAppCatalogPath(originalTempProcess.ProjectName);
            customerName = customerName.ToUpper().Replace(" ", "");

            string exportTablesCatalogFileName = packagesPath + "COMPONENTTABLES_" + customerName + ".CATALOG";

            if (!File.Exists(exportTablesCatalogFileName))
                exportTablesCatalogFileName = exportTablesCatalogFileName = packagesPath + "COMPONENTTABLES.CATALOG";

            var packagesContent = File.ReadAllLines(exportTablesCatalogFileName);

            for (int i = 0; i < packagesContent.Length; i++)
            {
                if (packagesContent[i].Length > 0)
                {
                    TempComponent = null;
                    tempArray = packagesContent[i].Split(',');
                    string componentName = tempArray[0];
                    string tableName = tempArray[1];
                    string tableDescription = tempArray[2];
                    TempComponent = originalTempProcess.ExportTablesComponents.Where(x => x.Name == componentName).FirstOrDefault();
                    if (TempComponent == null)
                        originalTempProcess.ExportTablesComponents.Add(new Component(componentName, tableName, tableDescription));
                    else
                        TempComponent.AddTable(tableName, tableDescription);
                }
            }
        }

        public static void GetImportTablesByCustomer(string customerName, string currentProjectName, Process originalTempProcess)
        {
            ImportComponent TempComponent;
            string[] tempArray;
            string packagesPath = Auxiliar.GetAppCatalogPath(originalTempProcess.ProjectName);
            customerName = customerName.ToUpper().Replace(" ", "");

            string importTablesCatalogFileName = packagesPath + "COMPONENTTABLES_" + customerName + ".CATALOG";

            if (!File.Exists(importTablesCatalogFileName))
                importTablesCatalogFileName = importTablesCatalogFileName = packagesPath + "COMPONENTTABLES.CATALOG";

            var packagesContent = File.ReadAllLines(importTablesCatalogFileName);

            for (int i = 0; i < packagesContent.Length; i++)
            {
                if (packagesContent[i].Length > 0)
                {
                    TempComponent = null;
                    tempArray = packagesContent[i].Split(',');
                    string componentName = tempArray[0];
                    string tableName = tempArray[1];
                    string tableDescription = tempArray[2];
                    TempComponent = originalTempProcess.ImportTablesComponents.Where(x => x.Name == componentName).FirstOrDefault();
                    if (TempComponent == null)
                        originalTempProcess.ImportTablesComponents.Add(new ImportComponent(componentName, tableName, tableDescription));
                    else
                        TempComponent.AddTable(tableName, tableDescription);
                }
            }
        }

        public static void SetupTables(string customerName, string projectName, Process originalTempProcess)
        {
            GetExportTablesByCustomer(customerName, projectName, originalTempProcess);
            GetImportTablesByCustomer(customerName, projectName, originalTempProcess);
        }

        public List<Process> GetProcesses()
        {
            string[] tempFiles;
            string processesPath, tcode = "", description = "", prepost = "";

            Process TempProcess;
            ProcessExecution TempProcessExecution;
            StepExecution tempStepExec;
            StatusExecution tempStatusExec;
            List<ProcessExecution> processListFromServer = GetProcessExecutions();
            //List<StepExecution> stepListFromServer = GetStepExecutions();
            List<StatusExecution> statusListFromServer = GetStatusExecutions();
            List<Process> output = new List<Process>();
            ServerSystem tempServer, dbserver;

            Auxiliar.SendLogRequest("Reading processes executions from path: " + Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser);

            foreach (Process processTemplate in Auxiliar.ProcessInitConfig)
            {
                processesPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_READ\\" + processTemplate.ProjectName + "\\PROCESSSUMARY\\";
                if (Directory.Exists(processesPath))
                {
                    tempFiles = Directory.GetFiles(processesPath, "*" + UserProfile.ItUser + ".json");
                    foreach (string fileName in tempFiles)
                    {
                        try
                        {
                            TempProcess = GetProcess(fileName);
                            if (TempProcess != null)
                            {
                                string acStatus = "\\\\10.130.19.40\\ait\\RUNTEAMCONSOLE\\AC_STATUS\\";

                                Process p = output.Where(x => x.Idx == TempProcess.Idx).FirstOrDefault();
                                                                
                                if (p == null)
                                {
                                    string acStatusFilePath = acStatus + UserProfile.ItUser.ToUpper() + "_" + TempProcess.Idx + "." + TempProcess.CurrentStatus;
                                    string acFileContent = TempProcess.Idx + "|" + TempProcess.CurrentStatus + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");

                                    File.WriteAllText(acStatusFilePath, acFileContent);

                                    if (TempProcess.BrideServer)
                                    {
                                        tempServer = new ServerSystem("GLOBAL", "GLB", "00", "syn99a08lp215", "BRIDGE", "PRD", "", "", "CI", "Linux", TempProcess.DBType, "");
                                    }
                                    else
                                    {
                                        tempServer = Auxiliar.SalServerList.Where(x => x.Hostname == TempProcess.PAS && x.SID == TempProcess.SID).FirstOrDefault();
                                    }

                                    if (tempServer != null)
                                    {
                                        TempProcess.Environment = tempServer.Environment;
                                    }
                                    else
                                    {
                                        if (TempProcess.CurrentStatus != "COMPLETED")
                                        {
                                            string title = TempProcess.Title;
                                            MessageBox.Show("Error loading process to main view.", title + " - " + TempProcess.Customer + " " + TempProcess.DBS, MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                        else
                                        {
                                            Auxiliar.RenamePreviousProcess(TempProcess);
                                        }
                                        break;
                                    }

                                    if (TempProcess.DBS != TempProcess.PAS)
                                    {
                                        dbserver = Auxiliar.SalServerList.Where(x => x.Hostname == TempProcess.DBS && x.SID == TempProcess.SID).FirstOrDefault();
                                        if (dbserver != null)
                                        {
                                            TempProcess.DBType = dbserver.DBType;
                                        }
                                    }
                                    else
                                    {
                                        TempProcess.DBType = tempServer.DBType;
                                    }
                                    
                                    for (int i = 0; i < processTemplate.StepList.Count; i++)
                                    {
                                        if (i < TempProcess.StepList.Count)
                                        {
                                            TempProcess.StepList[i].ProcessSelectedFlow = TempProcess.SelectedFlowMode;
                                            TempProcess.StepList[i].Flow = processTemplate.StepList[i].Flow;
                                            if (processTemplate.StepList[i].MaxTries != null)
                                            {
                                                TempProcess.StepList[i].MaxTries = processTemplate.StepList[i].MaxTries;
                                            }
                                        }
                                    }

                                    TransactionsPackage transactionsPackage = new TransactionsPackage();

                                    if (TempProcess.TransactionsPackages != null)
                                    {
                                        foreach (TransactionsPackage package in TempProcess.TransactionsPackages)
                                        {
                                            foreach (TransactionsPackage.Transaction transaction in package.Transactions)
                                            {
                                                if (processTemplate.TransactionsPackages != null)
                                                {
                                                    TransactionsPackage tempPackage = processTemplate.TransactionsPackages.Where(x => 
                                                    x.Transactions.Any(t => t.TCode == transaction.TCode)).FirstOrDefault();
                                                    if (tempPackage != null)
                                                    {
                                                        TransactionsPackage.Transaction tempTransaction = tempPackage.Transactions.Where(x => 
                                                        x.TCode == transaction.TCode).FirstOrDefault();
                                                        if (tempTransaction != null)
                                                        {
                                                            int postActIndex = transaction.TCode.IndexOf("RPOST");
                                                            if (postActIndex != -1)
                                                            {
                                                                tcode = transaction.TCode.Substring(0, transaction.TCode.Length - postActIndex);
                                                                prepost = "POST";
                                                            }
                                                            else
                                                            {
                                                                if (transaction.TCode.EndsWith("R"))
                                                                {
                                                                    tcode = transaction.TCode.Substring(0, transaction.TCode.Length - 1);
                                                                    prepost = "PRE";
                                                                }
                                                                else
                                                                {
                                                                    tcode = transaction.TCode;
                                                                }
                                                            }
                                                            description = tempTransaction.Description;
                                                        }
                                                    }
                                                }
                                                
                                                TransactionsPackage.Transaction t = new TransactionsPackage.Transaction() { Description = description, TCode = tcode, Selected = "", PrePost = prepost };
                                                
                                                if (transactionsPackage.Transactions == null)
                                                {
                                                    transactionsPackage.Transactions = new List<TransactionsPackage.Transaction>();
                                                }
                                                
                                                transactionsPackage.Transactions.Add(t);
                                                tcode = "";
                                                description = "";
                                                prepost = "";
                                            }
                                        }
                                    }
                                    
                                    TempProcess.SelectedTransactions = transactionsPackage;
                                    output.Add(TempProcess);
                                    TempProcessExecution = processListFromServer.Where(x => x.Idx == TempProcess.Idx).FirstOrDefault();
                                    //TempProcessExecution = Auxiliar.GetProcessExecution(TempProcess.Idx);
                                    if (TempProcessExecution == null)
                                    {
                                        CreateProcessExecutionRequest(TempProcess);
                                    }
                                    else
                                    {
                                        if (TempProcessExecution.CurrentStep != TempProcess.CurrentStepIndex)
                                        {
                                            TempProcessExecution.CurrentStep = TempProcess.CurrentStepIndex;
                                            try
                                            {
                                                Auxiliar.PutProcessTraking(TempProcessExecution, "ProcessExecutions/" + TempProcessExecution.Id);
                                            }
                                            catch (Exception ex) { }
                                        }
                                        //AQUÍ VOY
                                        foreach (Step step in TempProcess.StepList)
                                        {
                                            if (step.StatusList.Count > 0)
                                            {
                                                foreach (Status status in step.StatusList)
                                                {
                                                    tempStatusExec = statusListFromServer.Where(x => x.Idx == TempProcess.Idx && x.StepIndex == step.Index && x.State == status.State && x.DateTime == status.DateTime).FirstOrDefault();
                                                    
                                                    if (tempStatusExec == null)
                                                    {
                                                        StatusExecution se = new StatusExecution { Idx = TempProcess.Idx, StepIndex = step.Index, State = status.State, DateTime = status.DateTime };
                                                        try
                                                        {
                                                            Auxiliar.PostProcessTraking(se, "StatusExecutions/");
                                                        }
                                                        catch (Exception ex) { }
                                                    }
                                                }

                                                tempStepExec = Auxiliar.GetStepExecution(TempProcess.Idx, step.Index);
                                                if (tempStepExec == null)
                                                {
                                                    StepExecution s = new StepExecution { Idx = TempProcess.Idx, StepIndex = step.Index, Name = step.Name, Description = step.Description, Log = step.Log, Message = TempProcess.Message };
                                                    try
                                                    {
                                                        Auxiliar.PostProcessTraking(s, "StepExecutions/");
                                                    }
                                                    catch (Exception ex) { }
                                                }
                                                else
                                                {
                                                    if (tempStepExec.Log != step.Log)
                                                    {
                                                        tempStepExec.Log = step.Log;
                                                        try
                                                        {
                                                            Auxiliar.PutProcessTraking(tempStepExec, "StepExecutions/" + tempStepExec.Id);
                                                        }
                                                        catch (Exception ex) { }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string acStatusFilePath = acStatus + UserProfile.ItUser.ToUpper() + "_" + p.Idx + "." + p.CurrentStatus;
                                    string acFileContent = p.Idx + "|" + p.CurrentStatus + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");
                                    File.WriteAllText(acStatusFilePath, acFileContent);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Auxiliar.SendLogRequest("Error trying to load process: " + fileName + "|Error:" + e.Message);
                        }
                    }
                }
            }

            return output;
        }

        public Process GetProcess(string processFileName)
        {
            Process output = null;
            string idx = "";
            string json = "";
            if (Auxiliar.productiveRelease != "TEST")
            {
                 idx = processFileName.Split("\\")[8].Split(".")[0];
            }
            else
            {
                 idx = processFileName.Split("\\")[7].Split(".")[0];
            }

            if (File.Exists(processFileName))
            {
                try
                {
                    using (StreamReader r = new StreamReader(processFileName))
                    {
                        json = r.ReadToEnd();
                    }
                }
                catch (IOException)
                {
                    Auxiliar.SendLogRequest("Unable to read process file. " + processFileName);
                }
                catch (Exception)
                {
                    Auxiliar.SendLogRequest("Unable to read process file. " + processFileName);
                    if (MainWindow.PVMInstance != null)
                        Auxiliar.ShowPopupMessage(idx, "Unable to read this process-file: \n" + processFileName, "error");
                    else
                        MessageBox.Show("Unable to read this process-file: \n" + processFileName, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                try
                {
                    output = JsonConvert.DeserializeObject<Process>(json);
                }
                catch (Exception )
                {
                    Auxiliar.SendLogRequest("Unable to deserialize process file. " + processFileName);
                    if (MainWindow.PVMInstance != null)
                        Auxiliar.ShowPopupMessage(idx, "Unable to deserialize process file. " + processFileName, "error");
                    else
                        MessageBox.Show("Unable to deserialize process file. " + processFileName, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return output;
        }

        public static List<Favorites> GetFavorites()
        {
            string[] tempFiles;
            string favoritesPath;

            Favorites TempFavorite;
            List<Favorites> output = new List<Favorites>();

            foreach (Process processTemplate in Auxiliar.ProcessInitConfig)
            {
                favoritesPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "\\" + processTemplate.ProjectName + "\\FAVORITES\\";
                if (Directory.Exists(favoritesPath))
                {
                    tempFiles = Directory.GetFiles(favoritesPath, "*" + ".json");
                    foreach (string fileName in tempFiles)
                    {
                        TempFavorite = GetFav(fileName);
                        if (TempFavorite != null)
                        {
                            Favorites fav = output.Where(x => x.Name == TempFavorite.Name).FirstOrDefault();
                            if(fav == null)
                                output.Add(TempFavorite);

                            Process layout = Auxiliar.ProcessInitConfig.Where(x => x.Title == TempFavorite.Title).FirstOrDefault();
                            foreach (Process p in TempFavorite.Processes)
                            {
                                p.Credentials.OSUser = UserProfile.ItUser.ToLower();
                                if (layout != null)
                                {
                                    p.Title = layout.Title;
                                    p.IsSecuential = layout.IsSecuential;
                                    p.ApplReq = layout.ApplReq;
                                    p.SystemCopyModules = layout.SystemCopyModules;
                                }
                            }
                        }
                    }
                }
            }

            return output;
        }

        public static Favorites GetFav(string favFileName)
        {
            Favorites output = null;
            string json = "";

            try
            {
                using (StreamReader r = new StreamReader(favFileName))
                {
                    json = r.ReadToEnd();
                }
            }
            catch (Exception)
            {
                Auxiliar.SendLogRequest("Unable to read favorite file. " + favFileName);
                MessageBox.Show("Unable to read favorite file: \n" + favFileName, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                output = JsonConvert.DeserializeObject<Favorites>(json);
            }
            catch (Exception)
            {
                Auxiliar.SendLogRequest("Unable to deserialize favorite file. " + favFileName);
                MessageBox.Show("Unable to deserialize favorite file. " + favFileName, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return output;
        }

        public static void DiscardFav(Favorites fav)
        {
            string userProyectPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "\\" + fav.Processes.First().ProjectName + "\\FAVORITES\\";
            string discardPath = Auxiliar.CreateFolder(userProyectPath + "DISCARD\\");
            string favFileName = fav.Name + ".JSON";
            if(File.Exists(userProyectPath+ favFileName))
            {
                File.Move(userProyectPath + favFileName, discardPath + "\\" + Auxiliar.GenerateTimeStamp(Auxiliar.ConvertToEST()) + fav.Name + ".JSON");
            }
                
        }

        public List<ServerSystem> GetServerSystems()
        {
            List<ServerSystem> systems = new List<ServerSystem>();
            string salServersFile = Auxiliar.catalogPath + "CATALOG\\SALSERVERS.JSON";
            if (File.Exists(salServersFile))
            {
                FileInfo fi = new FileInfo(salServersFile);
                if (DateTime.Now < (fi.LastWriteTime.AddHours(12)))
                {
                    using (StreamReader r = new StreamReader(salServersFile))
                    {
                        string json = r.ReadToEnd();
                        systems = JsonConvert.DeserializeObject<List<ServerSystem>>(json);
                    }
                }
                else
                {
                    systems = RequestSalServersAndSave(salServersFile);
                    if (systems.Count == 0)
                    {
                        using (StreamReader r = new StreamReader(salServersFile))
                        {
                            try
                            {
                                string json = r.ReadToEnd();
                                systems = JsonConvert.DeserializeObject<List<ServerSystem>>(json);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error reading json file");
                            }
                        }
                    }
                }
            }
            else
            {
                systems = RequestSalServersAndSave(salServersFile);

            }

            if (systems.Count == 0)
            {
                MessageBoxResult result;
                do
                {
                    result = MessageBox.Show("Unable to get servers or write servers on file. " + salServersFile, "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                } while (result != MessageBoxResult.OK);

                Environment.Exit(0);
            }
            return systems;
        }

        public List<ServerSystem> RequestSalServersAndSave( string salServersFile)
        {
            List<ServerSystem> systems = new List<ServerSystem>();
            string salURL, jsonString;
            salURL = "http://sal.us.fit/sal/server/data?sEcho=1&iColumns=13&sColumns=&iDisplayStart=0&iDisplayLength=-1";

            try
            {
                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString(salURL);
                    var details = JObject.Parse(json);
                    var aaData = details.Last;
                    foreach (var server in aaData.First)
                    {
                        systems.Add(new ServerSystem(server));
                    }
                    jsonString = JsonConvert.SerializeObject(systems);
                    File.WriteAllText(salServersFile, jsonString);
                }
            }
            catch (Exception e)
            {
                Auxiliar.SendLogRequest("Unable to get servers or write servers from file. " + e.Message);
            }
            return systems;
        }

        public List<SaltMaster> GetMasterServers()
        {
            List<SaltMaster> masters = new List<SaltMaster>();
            string saltServersCatalogFile = Auxiliar.catalogPath + "CATALOG\\MASTERSALTCATALOG.CAT";
            if (File.Exists(saltServersCatalogFile))
            {
                string[] content = File.ReadAllLines(saltServersCatalogFile);
                foreach (string line in content)
                {
                    string[] tempData = line.Split("|");
                    SaltMaster master = new SaltMaster(tempData[0], tempData[1], tempData[2], tempData[3], tempData[4], tempData[5]);
                    masters.Add(master);
                }
            }
            return masters;
        }

        public List<ServerSystem> GetCloudServerSystems()
        {
            List<ServerSystem> servers = new List<ServerSystem>();
            string cloudServersFile = Auxiliar.catalogPath + "CATALOG\\SERVERSLIST_CLOUD.JSON";
            if (File.Exists(cloudServersFile))
            {
                using (StreamReader r = new StreamReader(cloudServersFile))
                {
                    try
                    {
                        string json = r.ReadToEnd();
                        servers = JsonConvert.DeserializeObject<List<ServerSystem>>(json);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error reading json file");
                    }
                }
            }
            return servers;
        }

        public static void ChangeStepConfig(string idx, string step, string config, string newVal)
        {
            var stepsConfigFileName = Auxiliar.GetConfingPath(idx) + idx + "_STEPS.CONF";
            if (File.Exists(stepsConfigFileName))
            {
                string[] contentList = File.ReadAllLines(stepsConfigFileName);
                if (contentList.Length > 0)
                {
                    var index = Array.FindIndex(contentList, x => x.Contains(step) != false);
                    string[] stepConfArr = contentList[index].Split("|");
                    string repeatAuto = "", repeatDate = "", repeatTime = "", tryNnum = "";
                    if (stepConfArr.Length >= 4)
                    {
                        repeatAuto = stepConfArr[3];
                        repeatDate = stepConfArr[4];
                        repeatTime = stepConfArr[5];
                        tryNnum = stepConfArr[6];
                    }
                    switch (config)
                    {
                        case "Auto":
                            contentList[index] = stepConfArr[0] + "|" + newVal + "|" + stepConfArr[2] + "|" + repeatAuto + "|" + repeatDate + "|" + repeatTime + "|" + tryNnum;
                            Auxiliar.SendLogRequest("Auto config changed|" + idx + "|setp " + step + "|flag " + newVal);
                            break;
                        case "Email":
                            contentList[index] = stepConfArr[0] + "|" + stepConfArr[1] + "|" + newVal + "|" + repeatAuto + "|" + repeatDate + "|" + repeatTime + "|" + tryNnum;
                            Auxiliar.SendLogRequest("Email config changed|" + idx + "|setp " + step + "|flag " + newVal);
                            break;
                        case "Reply":
                            contentList[index] = stepConfArr[0] + "|" + stepConfArr[1] + "|" + stepConfArr[2] + "|" + newVal + "|" + repeatDate + "|" + repeatTime + "|" + tryNnum;
                            Auxiliar.SendLogRequest("Reply config changed|" + idx + "|setp " + step + "|flag " + newVal);
                            break;
                        case "Try":
                            contentList[index] = stepConfArr[0] + "|" + stepConfArr[1] + "|" + stepConfArr[2] + "|" + repeatAuto + "|" + repeatDate + "|" + repeatTime + "|" + newVal;
                            Auxiliar.SendLogRequest("Try num changed|" + idx + "|setp " + step + "|flag " + newVal);
                            break;

                    }
                    if (contentList.Length > 0)
                    {
                        if (step.Contains("END"))
                        {
                            //string generalPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser;// + "\\" + idx + ".MONITOR";
                            string generalPath = Auxiliar.catalogPath + "AIT_" + UserProfile.ItUser + "_READ";
                            string filePath = generalPath + "\\" + idx + ".MONITOR";
                            string projectName = idx.Split("_")[2];
                            string fileToWrite = generalPath + "\\" + projectName + "\\PROCESSSUMARY\\" + idx + ".JSON";

                            //File.WriteAllLines(filePath, fileToWrite);
                            File.WriteAllText(filePath, fileToWrite);
                        }
                    }
                }
                else
                {
                    Auxiliar.SendLogRequest("WARNING!! _STEPS.CONF File with no content!|" + idx);

                    if (MainWindow.PVMInstance != null)
                        Auxiliar.ShowPopupMessage(idx, "An unexpected error has occured.\nPlease contact Innovation Team to the propper follow up", "error");
                    else
                        MessageBox.Show("An unexpected error has occured.\nPlease contact Innovation Team to the propper follow up", "Automation Console", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        public static void ChangeMAnualRestoreonfig(string idx)
        {
            var restoreConfigFileName = Auxiliar.GetConfingPath(idx) + idx + ".RESTOREMODE";
            if (File.Exists(restoreConfigFileName))
            {
                string[] contentList = File.ReadAllLines(restoreConfigFileName);
                contentList[1] = "MANUAL|";
                File.WriteAllLines(restoreConfigFileName, contentList);
            }
        }
        
        public void CreateProcessExecutionRequest(Process p)
        {
            ProcessExecution TempProcessExecution = new ProcessExecution { Idx = p.Idx, ProcessName = p.ProjectName, CurrentStep = p.CurrentStepIndex, GroupName = p.Team, User = p.User, PAS = p.PAS, DBS = p.DBS, SID = p.SID, Title = p.Description, Customer = p.Customer, CreateDate = Auxiliar.DateTimeFromTimeStamp(p.TimeStamp) };
            try
            {
                var response = Auxiliar.PostProcessTraking(TempProcessExecution, "ProcessExecutions/");
            }
            catch (Exception ex)
            {
            }
            foreach(Step step in p.StepList)
            {
                if(step.StatusList.Count > 0)
                {
                    foreach (Status status in step.StatusList)
                    {
                        StatusExecution se = new StatusExecution { Idx = p.Idx, StepIndex = step.Index, State = status.State, DateTime = status.DateTime };
                        try
                        {
                            Auxiliar.PostProcessTraking(se, "StatusExecutions/");
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    StepExecution s = new StepExecution { Idx = p.Idx, StepIndex = step.Index, Name = step.Name, Description = step.Description, Log = step.Log, Message = p.Message };
                    try
                    {
                        Auxiliar.PostProcessTraking(s, "StepExecutions/");
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        public List<ProcessExecution> GetProcessExecutions()
        {
            List<ProcessExecution> output;


            string urlReq = "";
            //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                urlReq = Auxiliar.monitorBkpUrl + "ProcessExecutions/GetUserProcessExecution/";
            else
                urlReq = Auxiliar.monitorUrl + "ProcessExecutions/GetUserProcessExecution/";

            WebRequest requestProcesses = WebRequest.Create(urlReq);

            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                requestProcesses.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                requestProcesses.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            // Get the response.
            WebResponse responseProcesses = requestProcesses.GetResponse();


            // Display the status.
            Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = responseProcesses.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                output = JsonConvert.DeserializeObject<List<ProcessExecution>>(responseFromServer);
            }

            // Close the response.
            responseProcesses.Close();
            return output;
        }

        public List<StepExecution> GetStepExecutions()
        {
            List<StepExecution> output;
            string urlReq = "";
            //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                urlReq = Auxiliar.monitorBkpUrl + "StepExecutions/";
            else
                urlReq = Auxiliar.monitorUrl + "StepExecutions/";

            WebRequest requestProcesses = WebRequest.Create(urlReq);

            // If required by the server, set the credentials.
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                requestProcesses.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                requestProcesses.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            // Get the response.
            WebResponse responseProcesses = requestProcesses.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = responseProcesses.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                output = JsonConvert.DeserializeObject<List<StepExecution>>(responseFromServer);
            }

            // Close the response.
            responseProcesses.Close();
            return output;
        }

        public List<StatusExecution> GetStatusExecutions()
        {
            List<StatusExecution> output;

            string urlReq = "";
            //if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-ROBEB" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
            if (UserProfile.ItUser.ToUpper() == "ITFLORR" || UserProfile.ItUser.ToUpper() == "ITVIVA" || UserProfile.ItUser.ToUpper() == "ITCANUA" || UserProfile.ItUser.ToUpper() == "ITRODRV" || UserProfile.ItUser.ToUpper() == "ITROBLB" || UserProfile.ItUser.ToUpper() == "VDC-FLORR" || UserProfile.ItUser.ToUpper() == "VDC-CANUA" || UserProfile.ItUser.ToUpper() == "VDC-RODRV" || UserProfile.ItUser.ToUpper() == "VDC-VIVAG")
                urlReq = Auxiliar.monitorBkpUrl + "StatusExecutions/GetUserStatusExecution/";
            else
                urlReq = Auxiliar.monitorUrl + "StatusExecutions/GetUserStatusExecution/";

            WebRequest requestProcesses = WebRequest.Create(urlReq);

            // If required by the server, set the credentials.
            // Configuration valid for local environment
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                requestProcesses.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                requestProcesses.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }

            // Get the response.
            WebResponse responseProcesses = requestProcesses.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = responseProcesses.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                output = JsonConvert.DeserializeObject<List<StatusExecution>>(responseFromServer);
            }

            // Close the response.
            responseProcesses.Close();
            return output;
        }

        public List<Maintenance> GetMaintenances()
        {
            List<Maintenance> output;

            WebRequest requestProcesses = WebRequest.Create(
             Auxiliar.serverURL + "Maintenances/GetMaintenances");

            // If required by the server, set the credentials.
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                requestProcesses.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                requestProcesses.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }
            // Get the response.
            WebResponse responseProcesses = requestProcesses.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = responseProcesses.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                output = JsonConvert.DeserializeObject<List<Maintenance>>(responseFromServer);
            }

            // Close the response.
            responseProcesses.Close();
            return output;
        }

        public List<BroadcastMessages> GetBroadcastMessages()
        {
            List<BroadcastMessages> output;

            WebRequest request = WebRequest.Create(
              Auxiliar.monitorUrl + "BroadcastMessages/GetUserBroadcastMessages/");

            // If required by the server, set the credentials.
            // Configuration valid for local environment
            if (Auxiliar.productiveRelease == "TEST")
            {
                // Configuration valid for local environment
                request.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                // Configuration valid for Cloned environment
                request.Credentials = new NetworkCredential(Auxiliar.vdcUser, Auxiliar.vdcPass);
            }
            try
            {
                // Get the response.
                WebResponse responseProcesses = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)responseProcesses).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = responseProcesses.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string responseFromServer = reader.ReadToEnd();
                    // Display the content.
                    output = JsonConvert.DeserializeObject<List<BroadcastMessages>>(responseFromServer);
                }

                // Close the response.
                responseProcesses.Close();
            }
            catch (Exception)
            {
                output = new List<BroadcastMessages>();
            }
            return output;
        }

    }
}
