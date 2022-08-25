namespace VMS_SURE.Accesscontrol
{
    partial class AccessControlSearchCard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textEditSearchCard = new DevExpress.XtraEditors.TextEdit();
            this.xtraUserControl1 = new DevExpress.XtraEditors.XtraUserControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DeviceId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DeviceName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControlUserTrans = new DevExpress.XtraGrid.GridControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSearchCard.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlUserTrans)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.textEditSearchCard);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(664, 59);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "ตรวจสอบสิทธิเข้าพื้นที่/ประตูจากบัตร";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(367, 30);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "ตรวจสอบ";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(62, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "หมายเลขบัตร";
            // 
            // textEditSearchCard
            // 
            this.textEditSearchCard.Location = new System.Drawing.Point(80, 31);
            this.textEditSearchCard.Name = "textEditSearchCard";
            this.textEditSearchCard.Size = new System.Drawing.Size(281, 20);
            this.textEditSearchCard.TabIndex = 0;
            // 
            // xtraUserControl1
            // 
            this.xtraUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraUserControl1.Location = new System.Drawing.Point(0, 59);
            this.xtraUserControl1.Name = "xtraUserControl1";
            this.xtraUserControl1.Size = new System.Drawing.Size(664, 387);
            this.xtraUserControl1.TabIndex = 1;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.DeviceId,
            this.DeviceName});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // DeviceId
            // 
            this.DeviceId.Caption = "หมายเลขเครื่อง";
            this.DeviceId.Name = "DeviceId";
            this.DeviceId.Visible = true;
            this.DeviceId.VisibleIndex = 0;
            this.DeviceId.Width = 302;
            // 
            // DeviceName
            // 
            this.DeviceName.Caption = "ชื่อเครื่อง";
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.Visible = true;
            this.DeviceName.VisibleIndex = 1;
            this.DeviceName.Width = 344;
            // 
            // gridControl1
            // 
            this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridControl1.Location = new System.Drawing.Point(0, 59);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(664, 163);
            this.gridControl1.TabIndex = 2;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridView2.GridControl = this.gridControlUserTrans;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "หมายเลขเครื่อง";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 152;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "ชื่อเครื่อง";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 156;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "วัน เวลา บันทึก";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 338;
            // 
            // gridControlUserTrans
            // 
            this.gridControlUserTrans.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridControlUserTrans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlUserTrans.Location = new System.Drawing.Point(0, 222);
            this.gridControlUserTrans.MainView = this.gridView2;
            this.gridControlUserTrans.Name = "gridControlUserTrans";
            this.gridControlUserTrans.Size = new System.Drawing.Size(664, 224);
            this.gridControlUserTrans.TabIndex = 3;
            this.gridControlUserTrans.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // AccessControlSearchCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 446);
            this.Controls.Add(this.gridControlUserTrans);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.xtraUserControl1);
            this.Controls.Add(this.groupControl1);
            this.Name = "AccessControlSearchCard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccessControlSearchCard";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditSearchCard.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlUserTrans)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEditSearchCard;
        private DevExpress.XtraEditors.XtraUserControl xtraUserControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn DeviceId;
        private DevExpress.XtraGrid.Columns.GridColumn DeviceName;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.GridControl gridControlUserTrans;
    }
}