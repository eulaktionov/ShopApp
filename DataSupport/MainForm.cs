using ShopData;
using static DataSupport.Properties.Resources;

namespace DataSupport
{
    public partial class MainForm : Form
    {
        string fileName = "Shop.xml";
        XmlFile xmlFile;

        AppMenu menu;
        Shop shop;

        public MainForm()
        {
            InitializeComponent();

            xmlFile = new XmlFile(fileName);
            shop = xmlFile.Open();

            MakeControls();
            SetForm();

            Load += MainForm_Load;
            FormClosed += (s, e) => Save();
        }

        void MakeControls()
        {
            menu = new()
            {
                Close = () => Close(),
                Open = ShowChildForm,
                Save = Save
            };
            Controls.Add(menu);
        }

        void SetForm()
        {
            (Text, Icon) = (AppTitle, AppIcon);
            IsMdiContainer = true;
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            SignForm form = new SignForm(shop.Customers);
            if(form.ShowDialog() == DialogResult.Cancel)
            {
                Close();
            }
        }

        void Save() => xmlFile.Save(shop);

        void ShowChildForm(object? menuCommand)
        {
            ToolStripMenuItem command = menuCommand as ToolStripMenuItem;
            var form = MdiChildren.FirstOrDefault(f => f.Tag == command);
            if(form == null)
            {
                string title = command.Text.Replace("&", string.Empty);
                form = (command.Tag as Type).Name switch
                {
                    "GroupsForm" => new GroupsForm(shop.Groups, menu)
                    { Text = title },
                    "ProductsForm" => new ProductsForm(shop.Products, menu, shop.Groups) 
                    { Text = title },
                    "CustomersForm" => new CustomersForm(shop.Customers, menu) 
                    { Text = title }
                };
                form.MdiParent = this;
                form.Tag = command;
                form.Show();
            }
            if(form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;
            }
            form.Activate();
        }
    }
}