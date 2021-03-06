﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReportsSchema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCreateSchema_Click(object sender, RoutedEventArgs e)
        {
            // Connect to service
            string loginMsg = "";
            ReportsService.ReportsasmxClient rclient = new ReportsService.ReportsasmxClient();
            rclient.Logon("sid@wolf.co.uk", "Wolfwolf8!", "", ref loginMsg);
            ReportsService.RaceCardData rcd = rclient.GetRaceCard(37);

            // Initialize string to be written to the file
            string writeToFile = "<RaceCardData>\n";

            // Append string with the data to be written
            foreach (DataTable table in rcd.Tables)
            {
                writeToFile += "\t<" + table.TableName.ToString() + ">\n";
                foreach (DataRow row in table.Rows)
                {
                    if (table.Rows.IndexOf(row) != 0)
                    {
                        writeToFile += "\n";
                    }
                    foreach (DataColumn column in table.Columns)
                    {
                        writeToFile += "\t\t<" + column.ColumnName.ToString() + ">";
                        writeToFile += row[column.ColumnName].ToString();
                        writeToFile += "</" + column.ColumnName.ToString() + ">\n";
                    }
                }
                writeToFile += "\t</" + table.TableName.ToString() + ">\n";
            }

            writeToFile += "</RaceCardData>";

            // Open stream
            System.IO.StreamWriter file = new System.IO.StreamWriter("ReportsSchemaAndData.xml");

            // Write to file
            file.WriteLine(writeToFile);

            // Close file
            file.Close();

            // Display success
            MessageBox.Show("Success");

            // Exit app
            this.Close();
        }
    }
}
