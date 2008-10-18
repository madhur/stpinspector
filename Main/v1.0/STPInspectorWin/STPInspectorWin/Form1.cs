using System;
using System.Collections.Generic;
using System.Windows.Forms;
using STPInspectorLib;

namespace STPInspectorWin
{
    /// <summary>
    /// Main form of the application
    /// </summary>
    public partial class Form1 : Form
    {
        private string _cabPath;

        /// <summary>
        /// Stores the path to current cabinet file
        /// </summary>
        public string CabFilePath
        {
            get
            {
                return _cabPath;
            }
            set
            {
                _cabPath = value;
            }

        }
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event of the Browse button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = Constants.fileExt;
            fileDialog.Filter = "Site Template files (*.stp)|*.stp|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
               
                        CabFilePath = fileDialog.FileName;
                        
                        txtPath.Text = CabFilePath;
                        cblSiteFeatures.Items.Clear();
                        cblWebFeatures.Items.Clear();
                        
                    
                
            }

            

        }
        
        /// <summary>
        /// Binds the two list boxes to the respective features
        /// </summary>
        /// <param name="cblFeatures"></param>
        /// <param name="items"></param>
        private void BindListBox(CheckedListBox cblFeatures, List<FeatureItem> items)
        {
            foreach (FeatureItem item in items)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    cblFeatures.Items.Add(item.ID, false);
                }
                else
                {
                    cblFeatures.Items.Add(item.Name, true);
                }
            }

        }

        /// <summary>
        /// Handles the click event of Inspect button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInspect_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(CabFilePath))
            {
                return ;
            }

            CabUtil cabUtil=new CabUtil();

            try
            {
                bool unresolvedWeb, unresolvedSite;
                string manifestPath=cabUtil.ExtractManifest(CabFilePath, System.IO.Path.GetTempPath());

                List<FeatureItem> items = ManifestLib.GetWebFeatures(manifestPath,true);
                List<FeatureItem> siteItems = ManifestLib.GetWebFeatures(manifestPath, false);

                unresolvedWeb=FeatureLib.ResolveFeatures(items);
                unresolvedSite=FeatureLib.ResolveFeatures(siteItems);

                BindListBox(cblWebFeatures, items);
                BindListBox(cblSiteFeatures, siteItems);

                cblWebFeatures.SelectionMode = SelectionMode.None;
                cblSiteFeatures.SelectionMode = SelectionMode.None;

                if (unresolvedWeb || unresolvedSite)
                {
                    lblStatus.Text = Constants.unResolvedStatus;
                }
                else
                {
                    lblStatus.Text = Constants.resolvedStatus;
                }

            }
            catch (NotValidCabinetException)
            {
                MessageBox.Show(Constants.invalidCabinet);
            }
            catch (ManifestNotFoundException)
            {
                MessageBox.Show(Constants.invalidCabinet);
            }
          
            finally
            {
                CabUtil.DeleteFile(System.IO.Path.GetTempPath());
            }

        }
        
        /// <summary>
        /// Handles the Load Event of the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
