using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Codentia.Common.Reporting.BL;
using Codentia.Common.WebControls;
using Codentia.Common.WebControls.HtmlElements;
using Codentia.Common.WebControls.Validators;
using Microsoft.Reporting.WebForms;

namespace Codentia.Common.Reporting.WebControls
{
    /// <summary>
    /// This control combines a standard .net ReportViewer with a Parameter Div control to render reports
    /// </summary>
    [ToolboxData("<{0}:CEReportViewer runat=server></{0}:CEReportViewer>")]
    public class CEReportViewer : CECompositeControl
    {
        private string _reportCode = string.Empty;
        private int _reportViewerWidthPercentage = 100;
        private string _calendarImageURL = string.Empty;
        private bool _hasValidators = false;
        private bool _rdlFormat = true;
        private bool _areParametersPopulated = false;
        private bool _isInWebPage = true;
        private CEReport _rep = null;
        private string _radioButtonCSSClass = string.Empty;
        private string _dateCSSClass = string.Empty;
        private bool _autoGenerateIfNoParameters;
        private bool _hasParameters = false;

        /// <summary>
        /// Gets or sets ReportCode
        /// </summary>
        public string ReportCode
        {
            get
            {
                return _reportCode;
            }

            set
            {
                _reportCode = value;
            }
        }

        /// <summary>
        /// Gets or sets ReportViewerWidth
        /// </summary>
        public int ReportViewerWidthPercentage
        {
            get
            {
                return _reportViewerWidthPercentage;
            }

            set
            {
                _reportViewerWidthPercentage = value;
            }
        }    
    
        /// <summary>
        /// Gets or sets CalendarImageURL
        /// </summary>
        public string CalendarImageURL
        {
            get
            {
                return _calendarImageURL;
            }

            set
            {
                _calendarImageURL = value;
            }
        }

        /// <summary>
        /// Gets or sets RadioButtonCSSClass
        /// </summary>
        public string RadioButtonCSSClass
        {
            get
            {
                return _radioButtonCSSClass;
            }

            set
            {
                _radioButtonCSSClass = value;
            }
        }

        /// <summary>
        /// Gets or sets DateCSSClass
        /// </summary>
        public string DateCSSClass
        {
            get
            {
                return _dateCSSClass;
            }

            set
            {
                _dateCSSClass = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [auto generate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [auto generate]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoGenerate
        {
            get
            {
                return _autoGenerateIfNoParameters;
            }

            set
            {
                _autoGenerateIfNoParameters = value;
            }
        }

        /// <summary>
        /// Create PDF
        /// </summary>
        /// <param name="pdfFilePath">file path of pdf</param>
        /// <param name="parameterValues">parameter Values</param>
        public void CreatePDF(string pdfFilePath, Dictionary<string, object> parameterValues)
        {
            _isInWebPage = false;
            EnsureCEReport();
            EnsureChildControls();
            ReportViewer rv = (ReportViewer)this.FindChildControl("RV");
            SetDataSources(rv, parameterValues);
            
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            using (FileStream fs = new FileStream(pdfFilePath, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Create child controls
        /// </summary>
        protected override void CreateChildControls()
        {
            if (!string.IsNullOrEmpty(_reportCode))
            {
                EnsureCEReport();

                _rdlFormat = !_rep.IsRdlc;

                if (_rep.ParametersAreRendered)
                {
                    Panel p = new Panel();
                    p.ID = "parameterPanel";
                    PopulateParameterPanels(p);
                    this.Controls.Add(p);
                }

                if (this.ShowControlsAsButtons)
                {
                    IdempotentButton b = new IdempotentButton();
                    b.ID = "generateButton";
                    b.Text = "Generate Report";
                    b.Click += new EventHandler(GenerateReportClick);
                    b.CausesValidation = true;
                    b.ValidationGroup = this.ID;
                    this.Controls.Add(b);
                }
                else
                {
                    P linkContainer = new P();

                    IdempotentLinkButton lb = new IdempotentLinkButton();
                    lb.ID = "generateButton";
                    lb.Text = "Generate Report";
                    lb.Click += new EventHandler(GenerateReportClick);
                    lb.CausesValidation = true;
                    lb.ValidationGroup = this.ID;
                    linkContainer.Controls.Add(lb);

                    this.Controls.Add(linkContainer);
                }

                Panel repvPanel = new Panel();
                repvPanel.ID = "RVPanel";
                this.Controls.Add(repvPanel);

                ReportViewer rv = new ReportViewer();
                rv.ID = "RV";
                rv.ProcessingMode = ProcessingMode.Local;
                rv.LocalReport.EnableExternalImages = true;
                rv.LocalReport.EnableHyperlinks = true;

                if (_rdlFormat)
                {
                    // stream from an rdl file   
                    StringReader sr = null;
                    if (_isInWebPage)
                    {
                        sr = _rep.GetRdlStringReader(Path.GetDirectoryName(this.Page.Request.PhysicalPath));

                        // sr = _rep.GetRdlStringReader(this.Page.Request.PhysicalApplicationPath);
                    }
                    else
                    {
                        sr = _rep.GetRdlStringReader(System.Environment.CurrentDirectory);
                    }

                    rv.LocalReport.LoadReportDefinition(sr);
                    sr.Close();
                }
                else
                {
                    string rdlSubFolder = string.Empty;
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportSubFolder"]))
                    {
                        rdlSubFolder = string.Format(@"{0}\", ConfigurationManager.AppSettings["ReportSubFolder"]);
                    }

                    // load direct from an rdlc file
                    string rdlcPath = string.Format("{0}\\{1}{2}.rdlc", this.Page.Request.PhysicalApplicationPath, rdlSubFolder, _rep.RdlName);
                    rv.LocalReport.ReportPath = rdlcPath;
                }

                rv.ShowParameterPrompts = false;
                rv.ShowReportBody = false;
                repvPanel.Controls.Add(rv);

                if (_autoGenerateIfNoParameters)
                {
                    if (!_hasParameters)
                    {
                        Page.Validate();
                        GenerateReportClick(null, null);

                        this.FindControl("generateButton").Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Generate Report Click event
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event args</param>
        protected void GenerateReportClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                EnsureCEReport();

                Dictionary<string, object> parameterValues = new Dictionary<string, object>();

                IEnumerator<string> ie = _rep.CEReportDataSources.Keys.GetEnumerator();

                // Ensure parameters are populated
                while (ie.MoveNext())
                {
                    CEReportDataSource mrds = _rep.CEReportDataSources[ie.Current];

                    if (mrds.CEReportParameters.Count > 0)
                    {
                        PopulateParameterValues(parameterValues);
                    }
                }

                GenerateLocalReport(parameterValues);
            }
        }   

        /// <summary>
        /// Validate checkboxes
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="args">Event Args</param>
        protected void checkBoxValidator_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            CustomValidator cv = (CustomValidator)sender;

            args.IsValid = false;

            UnorderedCheckBoxListSelectable ucbl = (UnorderedCheckBoxListSelectable)this.FindChildControl(cv.ControlToValidate);

            for (int i = 0; i < ucbl.UnorderedCheckBoxList.Items.Count; i++)
            {
                if (ucbl.UnorderedCheckBoxList.Items[i].Selected)
                {
                    args.IsValid = true;
                }
            }
        }

        /// <summary>
        /// Save ViewState
        /// </summary>
        /// <returns>view state object</returns>
        protected override object SaveViewState()
        {
            return _reportCode;
        }

        /// <summary>
        /// Load ViewState
        /// </summary>
        /// <param name="savedState">saved sate object</param>
        protected override void LoadViewState(object savedState)
        {
            _reportCode = Convert.ToString(savedState);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.DataBinding"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);

            if (_autoGenerateIfNoParameters)
            {
                if (!_hasParameters)
                {
                    Page.Validate();
                    GenerateReportClick(null, e);

                    this.FindControl("generateButton").Visible = false;
                }
            }
        }
        
        private void EnsureCEReport()
        {
            if (_rep == null)
            {
                _rep = new CEReport(_reportCode);
            }
        }

        private void GenerateLocalReport(Dictionary<string, object> parameterValues)
        {
            // Configure ReportViewer
            ReportViewer rv = (ReportViewer)this.FindChildControl("RV");

            EnsureCEReport();

            // allow hyperlinks
            rv.LocalReport.EnableHyperlinks = true;
            rv.LocalReport.EnableExternalImages = true;
            rv.HyperlinkTarget = "_blank";

            SetDataSources(rv, parameterValues);
            
            rv.Width = Unit.Percentage(_reportViewerWidthPercentage);           
            rv.LocalReport.Refresh();

            rv.LocalReport.DisplayName = _reportCode;

            // disable the print button as it is not cross browser and requires client pack install
            rv.ShowPrintButton = false;

            rv.ShowReportBody = true;
        }

        private void SetDataSources(ReportViewer rv, Dictionary<string, object> parameterValues)
        {
            DataSet ds = _rep.GetReportResultDataSet(parameterValues);
            rv.LocalReport.DataSources.Clear();

            for (int i = 0; i < ds.Tables.Count; i++)
                {
                    // Add Data Sources                                       
                    DataTable dt = ds.Tables[i];
                    ReportDataSource rds = new ReportDataSource(dt.TableName, dt);
                    rv.LocalReport.DataSources.Add(rds);
                }

            if (_rdlFormat)
            {               
                // Set Parameters to CollatedParameters For Report
                rv.LocalReport.SetParameters(_rep.CollatedReportParameters);                
            }
        }

        private void PopulateParameterValues(Dictionary<string, object> parameterValues)
        {
            if (!_areParametersPopulated)
            {
                IEnumerator<string> ie = _rep.CEReportDataSources.Keys.GetEnumerator();

                while (ie.MoveNext())
                {
                    CEReportDataSource mrds = _rep.CEReportDataSources[ie.Current];

                    if (mrds.CEReportParameters.Count > 0)
                    {
                        IEnumerator<string> ieP = mrds.CEReportParameters.Keys.GetEnumerator();
                        while (ieP.MoveNext())
                        {
                            CEReportParameter param = mrds.CEReportParameters[ieP.Current];

                            switch (param.ReportParameterType)
                            {
                                case ReportParameterType.CheckBox:
                                    parameterValues[ieP.Current] = GetCheckBoxPanelValue(param);
                                    break;

                                case ReportParameterType.Date:
                                    parameterValues[ieP.Current] = GetDatePanelValue(param);
                                    break;

                                case ReportParameterType.TextBox:
                                    parameterValues[ieP.Current] = GetTextBoxPanelValue(param);
                                    break;

                                case ReportParameterType.DropDown:
                                    parameterValues[ieP.Current] = GetDropDownPanelValue(param);
                                    break;

                                case ReportParameterType.RadioButton:
                                    parameterValues[ieP.Current] = GetRadioButtonPanelValue(param);
                                    break;
                            }
                        }
                    }
                }

                _areParametersPopulated = true;
            }
        }
        
        private void PopulateParameterPanels(Panel parameterPanel)
        {         
            IEnumerator<string> ie = _rep.CEReportDataSources.Keys.GetEnumerator();

            while (ie.MoveNext())
            {
                CEReportDataSource mrds = _rep.CEReportDataSources[ie.Current];

                if (mrds.CEReportParameters.Count > 0)
                {
                    _hasParameters = true;
 
                    IEnumerator<string> ieP = mrds.CEReportParameters.Keys.GetEnumerator();
                    while (ieP.MoveNext())
                    {
                        CEReportParameter param = mrds.CEReportParameters[ieP.Current];
                        if (param.IsRendered)
                        {
                            DataView dv = new DataView();

                            if (param.ReportParameterType != ReportParameterType.Date)
                            {
                                dv = param.GetParameterDataView();
                            }

                            Panel paramPanel = new Panel();
                            paramPanel.ID = string.Format("{0}panel", param.ParameterCode);

                            switch (param.ReportParameterType)
                            {
                                case ReportParameterType.CheckBox:
                                    PopulateCheckBoxPanel(param, paramPanel, dv);
                                    break;

                                case ReportParameterType.Date:
                                    PopulateDatePanel(param, paramPanel);
                                    break;

                                case ReportParameterType.TextBox:
                                    // Note CSS Class support should be added at some stage
                                    PopulateTextBoxPanel(param, paramPanel, string.Empty);
                                    break;

                                case ReportParameterType.DropDown:
                                    PopulateDropDownPanel(param, paramPanel, dv);
                                    break;

                                case ReportParameterType.RadioButton:
                                    PopulateRadioButtonPanel(param, paramPanel, dv);
                                    break;
                            }

                            parameterPanel.Controls.Add(paramPanel);
                        }
                    }
                }

                if (_hasValidators)
                {
                    ValidationSummary vs = new ValidationSummary();
                    vs.ValidationGroup = this.ID;
                    parameterPanel.Controls.Add(vs);
                }
            }
        }

        private object GetCheckBoxPanelValue(CEReportParameter param)
        {
            UnorderedCheckBoxListSelectable ucbl = (UnorderedCheckBoxListSelectable)this.FindChildControl(param.ParameterCode);
            StringBuilder sb = new StringBuilder();
            sb.Append("<root>");
            for (int i = 0; i < ucbl.UnorderedCheckBoxList.Items.Count; i++)
            {
                ListItem li = ucbl.UnorderedCheckBoxList.Items[i];
                string state = "off";
                if (li.Selected)
                {
                    state = "on";
                }

                sb.AppendFormat(@"<cb id='{0}' value='{1}'/>", li.Value, state);
            }

            sb.Append("</root>");
            return sb.ToString();
        }

        private object GetDropDownPanelValue(CEReportParameter param)
        {
            DropDownList dd = (DropDownList)this.FindChildControl(param.ParameterCode);                        
            return dd.SelectedValue;
        }

        private object GetTextBoxPanelValue(CEReportParameter param)
        {
            TextBox tb = (TextBox)this.FindChildControl(param.ParameterCode);
            return tb.Text;
        }

        private object GetRadioButtonPanelValue(CEReportParameter param)
        {
            UnorderedRadioButtonList urbl = (UnorderedRadioButtonList)this.FindChildControl(param.ParameterCode);
            string value = string.Empty;
            for (int i = 0; i < urbl.Items.Count; i++)
            {
                ListItem li = urbl.Items[i];
                if (li.Selected)
                {
                    value = li.Value;
                }                
            }

            if (param.DbType == DbType.Boolean)
            {
                return value == "-1";
            }

            return value;
        }

        private object GetDatePanelValue(CEReportParameter param)
        {
            TextBox tb = (TextBox)this.FindChildControl(param.ParameterCode);
            if (string.IsNullOrEmpty(tb.Text))
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(tb.Text);
            }
        }

        private void PopulateDropDownPanel(CEReportParameter param, Panel p, DataView dv)
        {
            Label l = new Label();
            l.Text = param.Caption;
            p.Controls.Add(l);

            DropDownList dd = new DropDownList();
            dd.DataSource = dv;
            dd.ID = param.ParameterCode;
            dd.DataTextField = "Name";
            dd.DataValueField = "Value";
            dd.DataBind();
            dd.SelectedIndex = 0;
            
            if (param.DefaultValue != string.Empty)
            {
                dd.SelectedValue = param.DefaultValue;
            }

            dd.EnableViewState = true;

            l.AssociatedControlID = dd.ID;
            p.Controls.Add(dd);
        }

        private void PopulateDatePanel(CEReportParameter param, Panel p)
        {
            PopulateTextBoxPanel(param, p, _dateCSSClass);
        }

        private void PopulateTextBoxPanel(CEReportParameter param, Panel p, string cssClass)
        {
            Label l = new Label();
            l.Text = param.Caption;

            TextBox tb = new TextBox();
            tb.ID = param.ParameterCode;
            tb.CssClass = cssClass;

            // do not add validator if needed
            if (!param.IsNullAllowed)
            {
                _hasValidators = true;
                RequiredFieldValidator rv = new RequiredFieldValidator();
                rv.ID = string.Format("{0}_rv", param.ParameterCode);
                rv.ValidationGroup = this.ID;
                rv.ControlToValidate = tb.ID;
                rv.Text = "*";
                rv.ErrorMessage = param.ErrorMessageCaption;
            }

            p.Controls.Add(l);
            p.Controls.Add(tb);
        }

        private void PopulateCheckBoxPanel(CEReportParameter param, Panel p, DataView dv)
        {
            Label l = new Label();
            l.Text = param.Caption;
                        
            UnorderedCheckBoxListSelectable ucbl = new UnorderedCheckBoxListSelectable();
            ucbl.ID = param.ParameterCode;
            l.AssociatedControlID = ucbl.ID;

            for (int i = 0; i < dv.Table.Rows.Count; i++)
            {
                DataRow dr = dv.Table.Rows[i];

                ListItem li = new ListItem(Convert.ToString(dr["Name"]), Convert.ToString(dr["Value"]));

                ucbl.UnorderedCheckBoxList.Items.Add(li);
            }
            
            p.Controls.Add(l);
            p.Controls.Add(ucbl);

            // add a validator to ensure that at least one checkbox is selected
            ServerValidateEventHandler eh = new ServerValidateEventHandler(checkBoxValidator_ServerValidate);
            CECustomValidator cv = new CECustomValidator("customCheckBoxValidator", ucbl.ID, "At least one checkbox must be selected", this.ID, false, eh);
            p.Controls.Add(cv);                                                        
        }

        private void PopulateRadioButtonPanel(CEReportParameter param, Panel p, DataView dv)
        {
            Label l = new Label();
            l.Text = param.Caption;
            p.Controls.Add(l);

            UnorderedRadioButtonList urbl = new UnorderedRadioButtonList();
            urbl.CssClass = _radioButtonCSSClass;
            urbl.ID = param.ParameterCode;

            for (int i = 0; i < dv.Table.Rows.Count; i++)
            {
                DataRow dr = dv.Table.Rows[i];

                ListItem li = new ListItem(Convert.ToString(dr["Name"]), Convert.ToString(dr["Value"]));

                if (param.DefaultValue != string.Empty)
                {
                    if (param.DefaultValue == Convert.ToString(dr["Value"]))
                    {
                        li.Selected = true;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        li.Selected = true;
                    }
                }
                
                urbl.Items.Add(li);
            }

            l.AssociatedControlID = urbl.ID;
            p.Controls.Add(urbl);
        }
    } 
}
