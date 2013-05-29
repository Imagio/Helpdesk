using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Imagio.Helpdesk.Model;
using Word = Microsoft.Office.Interop.Word;

namespace Imagio.Helpdesk.ViewModel.Helper
{
    public static class Report
    {
        private static void MSWordStarter(string template, Action<Word.Document> execute)
        {
            Word.Application mApl = null;
            try
            {
                mApl = new Word.Application();
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Templates\\" + template;
                Word.Document doc = mApl.Documents.Add(Template: path);
                execute(doc);
                doc.Fields.Update();
                doc.Saved = true;
                mApl.Visible = true;
                mApl.Activate();
            }
            catch(Exception error)
            {
                if (mApl != null)
                    (mApl as Microsoft.Office.Interop.Word._Application).Quit(SaveChanges: false);
                MessageBox.Show(error.Message, "Ошибка", MessageBoxButton.OK);
            }
        }

        public static void ReportConsumable(HelpdeskContext context)
        {
            var ca = EntityStore.GetEntityQuery<ConsumableAccounting>(context).OrderBy(o => o.Date);
            MSWordStarter(@"consumable.dot", doc =>
                {
                    var table = doc.Tables[1];
                    var i = 2;

                    foreach (var item in ca)
                    {
                        table.Rows.Add();
                        table.Cell(i, 1).Range.Text = item.Date.Value.ToShortDateString();
                        table.Cell(i, 2).Range.Text = item.Consumable.Name.ToString();
                        table.Cell(i, 3).Range.Text = item.Consumable.Article.ToString();
                        table.Cell(i, 4).Range.Text = (item.Sign ? "+" : "-") + item.Count.ToString();
                        i++;
                    }
                });
        }

        public static void ReportHistory(HelpdeskContext context)
        {
            var ca = EntityStore.GetEntityQuery<Hardware>(context);

            MSWordStarter(@"history.dot", doc =>
            {
                var table = doc.Tables[1];
                var i = 2;

                foreach (var item in ca)
                {
                    if (item.StartDate != null)
                    {
                        table.Rows.Add();
                        table.Cell(i, 1).Range.Text = item.StartDate.Value.ToShortDateString();
                        table.Cell(i, 2).Range.Text = "постановка на учет";
                        table.Cell(i, 3).Range.Text = item.Name;
                        table.Cell(i, 4).Range.Text = item.HardwareType == null ? "" : item.HardwareType.Name;
                        table.Cell(i, 5).Range.Text = item.Maker == null ? "" : item.Maker.Name;
                        table.Cell(i, 6).Range.Text = item.InventoryNumber;
                        i++;
                    }
                    if (item.EndDate != null)
                    {
                        table.Rows.Add();
                        table.Cell(i, 1).Range.Text = item.StartDate.Value.ToShortDateString();
                        table.Cell(i, 2).Range.Text = "списание";
                        table.Cell(i, 3).Range.Text = item.Name;
                        table.Cell(i, 4).Range.Text = item.HardwareType == null ? "" : item.HardwareType.Name;
                        table.Cell(i, 5).Range.Text = item.Maker == null ? "" : item.Maker.Name;
                        table.Cell(i, 6).Range.Text = item.InventoryNumber;
                        i++;
                    }
                }
            });
        }

        public static void ReportMove(HelpdeskContext context)
        {
            var ca = EntityStore.GetEntityQuery<Hardware>(context).OrderBy(o => o.Name);
            MSWordStarter(@"move.dot", doc =>
            {
                var table = doc.Tables[1];
                var i = 2;

                foreach (var item in ca)
                {
                    table.Rows.Add();
                    table.Cell(i, 1).Range.Text = item.Name;
                    table.Cell(i, 2).Range.Text = item.HardwareType == null ? "" : item.HardwareType.Name;
                    table.Cell(i, 3).Range.Text = item.Maker == null ? "" : item.Maker.Name;
                    table.Cell(i, 4).Range.Text = item.Master == null ? "" : item.Master.ShortName;
                    table.Cell(i, 5).Range.Text = item.InventoryNumber;
                    table.Cell(i, 6).Range.Text = item.StartDate == null ? "" : item.StartDate.Value.ToShortDateString();
                    table.Cell(i, 7).Range.Text = item.EndDate == null ? "" : item.StartDate.Value.ToShortDateString();
                    i++;
                }
            });
        }
    }
}
