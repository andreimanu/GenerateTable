using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Propuestas.util;

namespace Propuestas.Test {
    /// <summary>
    /// Interaction logic for GenerarTabla.xaml
    /// </summary>
    public partial class GenerarTabla : Window {
        private string TableName { get; set; }

        public GenerarTabla()
        {
            InitializeComponent();
            this.TableName = "LINALB";
        }
        public GenerarTabla(string TableName) {
            InitializeComponent();
            this.TableName = TableName.ToUpper();
        }

        private void generar_click(object sender, EventArgs e)
        {
            GenerateClass();
        }
        public void GenerateClass()
        {
            this.TableName = tbNombre.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(TableName))
                return;
            string colName;
            string typeName;
            string finType = "";
            StringBuilder Fields = new StringBuilder();
            Fields.Append("public string Fields = \"");
            StringBuilder sb = new StringBuilder();
            sb.Append("namespace Propuestas.Modelo {\n");
            sb.Append("\tpublic class g" + TableName + "{\n");

            string comando =
                "SELECT COLUMN_NAME, TYPE_NAME, DATA_TYPE FROM \"SYSIBM\".\"SQLCOLUMNS\" WHERE TABLE_NAME = '" +
                TableName + "' AND TABLE_SCHEM = 'META3'";
            App.Comando.CommandText = comando;
            Dictionary<string, string> cols = new Dictionary<string, string>();
            using (var reader = App.Comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    colName = reader.GetString(0).ToUpper();
                    typeName = reader.GetString(1).ToUpper();
                    if (typeName.Equals("DECIMAL"))
                        finType = "decimal";
                    if (typeName.Equals("CHAR") || typeName.Equals("VARCHAR"))
                        finType = "string";
                    sb.Append("\t\tpublic " + finType + " " + colName + " { get; set; }\n");
                    Fields.Append(colName + ",");
                    cols.Add(colName.Trim(),finType);
                }
            }
            
            Fields.Remove(Fields.Length - 1,1);
            sb.Append("\t\t" + Fields.ToString()+"\";\n");
            sb.Append("\t\tpublic g" + TableName + "(){\n");
            sb.Append("\t\t\tApp.Comando.CommandText = \"SELECT " + Fields.ToString().Replace("\"", "").Split('=')[1] + " FROM " + TableName + " WHERE \";\n");
            sb.Append("\t\t\tusing (var reader = App.Comando.ExecuteReader())\n");
            sb.Append("\t\t\t\twhile(reader.Read()){\n");
            string[] split = Fields.ToString().Split('=')[1].Replace("\"", "").Split(',');
            int i = 0;
            foreach (var part in split)
            {
                sb.Append("\t\t\t\t\tthis." + part.Trim() + " = reader.Get" + Util.UppercaseFirst(cols[part.Trim()]) + "(" + i + ");\n");
                i++;
            }
            sb.Append("\t\t\t}\n");
            sb.Append("\t\t}\n");
            sb.Append("\t}\n");
            sb.Append("}\n");
            System.IO.File.WriteAllText(@"g" + TableName + ".cs", sb.ToString());
        }
    }
}
