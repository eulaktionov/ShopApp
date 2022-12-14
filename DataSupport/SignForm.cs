using ShopData;
using static DataSupport.Properties.Resources;

namespace DataSupport
{
    public partial class SignForm : Form
    {
        const int shift = 20;

        LabelText nameEntry;
        LabelText passwordEntry;
        Button okButton;
        Button cancelButton;

        IdNameList<Customer> users;

        public SignForm(IdNameList<Customer> users)
        {
            InitializeComponent();
            this.users = users;

            MakeControls();
            SetForm();
        }
        
        void MakeControls()
        {
            nameEntry = new LabelText("&Имя:", 200,
                new Point(shift * 4, shift));
            Controls.Add(nameEntry);
            nameEntry.Value.Text = "Пользователь 2";    // ---

            passwordEntry = new LabelText("&Пароль:", 200,
                new Point(nameEntry.Left, nameEntry.Bottom +shift));
            passwordEntry.Value.PasswordChar = '#';
            Controls.Add(passwordEntry);
            passwordEntry.Value.Text = "222";           // ---

            okButton = new Button();
            okButton.Text = "Дальше";
            okButton.Location = 
                new Point(nameEntry.Left, passwordEntry.Bottom + 2 * shift);
            okButton.DialogResult = DialogResult.OK;
            okButton.Click += OkButton_Click;
            Controls.Add(okButton);

            cancelButton = new Button();
            cancelButton.Text = "Отмена";
            cancelButton.Location =
                new Point(okButton.Right + 2 * shift, okButton.Top);
            Controls.Add(cancelButton);
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            Customer user = users.FirstOrDefault
                (it => it.Name == nameEntry.Value.Text);
            if (user is null)
            {
                MessageBox.Show("Нет такого имени!", AppTitle);
                cancelButton.PerformClick();
            }
            else
            if (user.Password != passwordEntry.Value.Text)
            {
                MessageBox.Show("Неверен пароль!", AppTitle);
                cancelButton.PerformClick();
            }
        }

        void SetForm()
        {
            Icon = AppIcon;
            Text = "Введите имя и пароль";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Padding = new Padding(shift);

            Width = cancelButton.Right + shift * 6;
            Height = cancelButton.Bottom + shift * 4;

            AcceptButton = okButton;
            CancelButton = cancelButton;
        }
    }
}
