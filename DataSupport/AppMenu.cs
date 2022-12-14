using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    internal class AppMenu : MenuStrip
    {
        public Action<object> Open { get; init; }
        public Action Save { get; init; }
        public Action Close { get; init; }

        public Action? Add { get; set; }
        public Action? Remove { get; set; }

        public AppMenu() => Items.AddRange(new ToolStripItem[]
        {
            new ToolStripMenuItem("&Формы", null, new ToolStripItem[]
            {
                new ToolStripMenuItem("&Группы", null,
                (s, e) => Open?.Invoke(s)) { Tag = typeof(GroupsForm) },
                new ToolStripMenuItem("П&родукты", null,
                (s, e) => Open?.Invoke(s)) { Tag = typeof(ProductsForm) },
                new ToolStripMenuItem("&Пользователи", null,
                (s, e) => Open?.Invoke(s)) { Tag = typeof(CustomersForm) },
                new ToolStripSeparator(),
                new ToolStripMenuItem("&Сохранить", null,
                (s, e) => Save?.Invoke(), Keys.Control | Keys.S),
                new ToolStripSeparator(),
                new ToolStripMenuItem("&Выйти", null,
                (s, e) => Close?.Invoke())
            }),
            new ToolStripMenuItem("&Редактирование", null, new ToolStripItem[]
            {
                new ToolStripMenuItem("&Добавить", null,
                (s, e) => Add?.Invoke(), Keys.Control | Keys.A),
                new ToolStripMenuItem("&Удалить", null,
                (s, e) => Remove?.Invoke(), Keys.Control | Keys.D)
            })
        });
    }
}
