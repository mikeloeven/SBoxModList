using System;
using System.Collections.Generic;
using System.ComponentModel;
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
namespace SBoxModList
{
    public partial class Form1 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        WebClient web = new WebClient();
        XPathDocument SBCFile;
        XPathNavigator nav;
        XPathNodeIterator modListIt;
        
        public Form1()
        {
            ofd.Filter = "SBC|*.sbc";
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    
                    String fName = ofd.FileName;
                    String sFName = ofd.FileName;
                    SBCFile = new XPathDocument(fName);
                    nav = SBCFile.CreateNavigator();
                    modListIt = nav.Select("//ModItem/PublishedFileId");

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

                    







                }
            }
            catch (Exception)
            {

            }
      

        }

        private void txtOut_LinkClicked(Object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

    }
}
