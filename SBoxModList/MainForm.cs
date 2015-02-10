using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
namespace SBoxModList
{

    
    public partial class MainForm : Form
    {
        
        OpenFileDialog ofd = new OpenFileDialog();
        WebTimeout web = new WebTimeout();
        XPathDocument SBCFile;
        XPathNavigator nav;
        XPathNodeIterator modListIt;
        
        public MainForm()
        {
            ofd.Filter = "SBC|*.sbc";
            InitializeComponent();
        }

        public struct ModInfo
        {
            public String ID;
            public String title;
            public String URL;
        }
        public List<ModInfo> ModList = new List<ModInfo>();


        private void button1_Click(object sender, EventArgs e)
        {


            
            try
            {

                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    lblLoad.Text = "Loading: ";
                    String fName = ofd.FileName;
                    String sFName = ofd.FileName;
                    SBCFile = new XPathDocument(fName);
                    nav = SBCFile.CreateNavigator();
                    modListIt = nav.Select("//ModItem/PublishedFileId");
                    
                    /*
                    txtOutRAW.Clear();
                    
                    while (modListIt.MoveNext())
                    {
                        
                        txtOutRAW.AppendText(modListIt.Current.InnerXml + "\r\n");
                        

                    }
                    */



                    ModList.Clear();

                    while (modListIt.MoveNext())
                    {
                        ModInfo modInfo = new ModInfo();
                        modInfo.ID = modListIt.Current.InnerXml;
                        modInfo.URL = "http://steamcommunity.com/sharedfiles/filedetails/?id=" + modInfo.ID;
                        int failcount = 0;
                        do{
                        try
                        {
                            
                            string workshopPage = web.DownloadString(modInfo.URL);
                            System.Threading.Thread.Sleep(500);
                            modInfo.title = Regex.Match(workshopPage, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                            modInfo.title = modInfo.title.Replace("Steam Workshop :: ", "");
                            if (failcount > 0) failcount = 0;
                            
                        }
                        catch (Exception)
                        {
                            modInfo.title = "URL Timed Out";
                            failcount++;
                        }
                        }while(failcount>0&&failcount<4);
                        ModList.Add(modInfo);
                        lblLoad.Text += "|";
                        if(lblLoad.Text.TakeWhile(c => c == '|').Count()>10)
                        {
                            lblLoad.Text = "Loading: ";
                        }
                        
                        lblLoad.Refresh();
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(500);
                        
                        
                        


                    }

                    ModList = ModList.OrderBy(m => m.ID).ToList();

                    txtOutRAW.Clear();
                    txtOut.Clear();
                    foreach (ModInfo MI in ModList)
                    {
                        txtOutRAW.AppendText(MI.ID + "\r\n");
                        StringBuilder tOut = new StringBuilder();
                        tOut.Append("ModID: " + MI.ID + "       ");
                        tOut.Append("ModName: " + MI.title + "      ");
                        tOut.Append("Workshop URL: " + MI.URL + "\r\n");
                        txtOut.AppendText(tOut.ToString());
                    }
                    lblLoad.Text = "File Loaded";

                    //old code deprecated
                    /*
                    while (modListIt.MoveNext())
                    {
                        StringBuilder outputStr = new StringBuilder();
                        string ID = modListIt.Current.InnerXml;
                        outputStr.Append("ModID>>   " + ID);
                        string URL =  "http://steamcommunity.com/sharedfiles/filedetails/?id=" + ID;
                        try{
                            string workshopPage = web.DownloadString(URL);
                        string title = Regex.Match(workshopPage, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                        title = title.Replace("Steam Workshop :: ", "");
                        outputStr.Append("  >>NAME>>    " + title);
                        }
                        catch(Exception)
                        {
                            outputStr.Append("      >>NAME>>         BadURL");
                        }
                        outputStr.Append("  >>LINK>>    " + URL);
                        outputStr.Append("\r\n");
                        txtOut.AppendText(outputStr.ToString());
                                                
                    }

                    */







                }

                else { txtOutRAW.Text = "Unable To Read File"; }
                     

            }
            catch (Exception)
            {
                txtOutRAW.Text = "An Unknown Error Has Occurred";
            }
      

        }

        private void txtOut_LinkClicked(Object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void btnSName_Click(object sender, EventArgs e)
        {
            ModList = ModList.OrderBy(m => m.title).ToList();

            txtOutRAW.Clear();
            txtOut.Clear();
            foreach (ModInfo MI in ModList)
            {
                txtOutRAW.AppendText(MI.ID + "\r\n");
                StringBuilder tOut = new StringBuilder();
                tOut.Append("ModName: " + MI.title + "      ");
                tOut.Append("ModID: " + MI.ID + "       ");
                tOut.Append("Workshop URL: " + MI.URL + "\r\n");
                txtOut.AppendText(tOut.ToString());
            }
        }

        private void btnSID_Click(object sender, EventArgs e)
        {
            ModList = ModList.OrderBy(m => m.ID).ToList();

            txtOutRAW.Clear();
            txtOut.Clear();
            foreach (ModInfo MI in ModList)
            {
                txtOutRAW.AppendText(MI.ID + "\r\n");
                StringBuilder tOut = new StringBuilder();
                tOut.Append("ModID: " + MI.ID + "       ");
                tOut.Append("ModName: " + MI.title + "      ");
                tOut.Append("Workshop URL: " + MI.URL + "\r\n");
                txtOut.AppendText(tOut.ToString());
            }
        }

        

        

    }

    
}
