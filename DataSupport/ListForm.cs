using ShopData;
using static DataSupport.Properties.Resources;

namespace DataSupport
{
    internal partial class ListForm<T> : Form where T : IdName
    {
        protected int shift = 20;

        protected ListBox list;
        protected LabelText id; 
        protected LabelText name;

        IdNameList<T> data;
        protected BindingSource binding;

        public ListForm(IdNameList<T> data, AppMenu menu)
        {
            InitializeComponent();
            this.data = data;
            (menu.Add, menu.Remove) = (Add, Remove);

            MakeControls();
            SetForm();

            binding = new BindingSource();
            binding.DataSource = data;
            list.DataSource = binding;
            list.DisplayMember = "Name";
            list.ValueMember = "Id";    //?
            id.Value.DataBindings.Add("Text", binding, "Id");
            name.Value.DataBindings.Add("Text", binding, "Name");
        }

        void MakeControls()
        {
            list = new()
            {
                Width = 200,
                Dock = DockStyle.Left
            };
            Controls.Add(list);

            id = new LabelText("&Код", 200,
                new Point(list.Right + shift * 2, list.Top));
            id.Value.ReadOnly = true;
            Controls.Add(id);

            name = new LabelText("&Название", 200,
                new Point(list.Right + shift * 2, id.Bottom + shift));
            Controls.Add(name);
        }

        void SetForm()
        {
            Icon = AppIcon;
            Padding = new Padding(shift);
            Width = name.Right + shift * 2;
        }

        public virtual void Add() 
        {
            list.Focus();
            data.AddNew();
            list.SelectedIndex = list.Items.Count - 1;
            name.Value.Focus();
        }

        public virtual void Remove() 
        {
            if(list.SelectedIndex < 0) return;
            data.RemoveAt(list.SelectedIndex);
        }
    }

    internal class GroupsForm : ListForm<Group>
    {
        public GroupsForm(IdNameList<Group> data, AppMenu menu) 
            : base(data, menu) {}
    }

    internal class ProductsForm : ListForm<Product>
    {
        ComboBox combo;
        LabelText price;

        IdNameList<Group> groups;
        protected BindingSource bindingGroups;

        public ProductsForm(IdNameList<Product> data, AppMenu menu, 
            IdNameList<Group> groups) : base(data, menu) 
        {
            this.groups = groups;

            MakeControls();

            bindingGroups = new BindingSource();
            bindingGroups.DataSource = groups;
            combo.DataSource = bindingGroups;
            combo.DisplayMember = "Name";
            combo.ValueMember = "Id";
            price.Value.DataBindings.Add("Text", binding, "Price");

            list.SelectedIndexChanged += List_SelectedIndexChanged;
            Load += List_SelectedIndexChanged;
            combo.SelectionChangeCommitted += Combo_SelectionChangeCommitted;
        }

        private void Combo_SelectionChangeCommitted(object? sender, EventArgs e) =>
            (binding.Current as Product).Group = combo.SelectedItem as Group;

        private void List_SelectedIndexChanged(object? sender, EventArgs e)
        {
            int groupId = (list.SelectedItem as Product)?.Group?.Id ?? 0;
            if(groupId > 0)
            {
                for(int i = 0; i < groups.Count; i++)
                {
                    if(groups[i].Id == groupId)
                    {
                        combo.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        void MakeControls()
        {
            combo = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Left = name.Left,
                Top = name.Bottom + shift
            };

            Controls.Add(combo);

            price = new LabelText("&Цена", 200,
                new Point(name.Left, combo.Bottom + shift));
            Controls.Add(price);
        }
    }

    internal class CustomersForm : ListForm<Customer>
    {
        LabelText password;

        public CustomersForm(IdNameList<Customer> data, AppMenu menu) 
            : base(data, menu) => MakeControls();

        void MakeControls()
        {
            password = new LabelText("&Пароль", 200,
                new Point(name.Left, name.Bottom + shift));
            password.Value.DataBindings.Add("Text", binding, "Password");
            Controls.Add(password);
            name.Title = "&Имя";
        }
    }

    public class LabelText : Panel
    {
        Label label;
        TextBox text;

        public string Title { set => label.Text = value; }
        public TextBox Value => text;
        
        public LabelText(string textName, int width, Point leftTop)
        {
            label = new ();
            text = new ()
            {
                Width = width,
                Top = label.Bottom
            };
            Location = leftTop;
            Size = new(width, label.Height + text.Height);
            Title = textName;

            Controls.Add(label);
            Controls.Add(text);
        }
    }
}
