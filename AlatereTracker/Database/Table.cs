using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlatereTracker.Database
{
    public class Table
    {
        public Table(EntityDescriptor descriptor, IEnumerable<Dictionary<string, string>> data = null) 
        {
            if (data == null) return;

            this.EntityDescriptor = descriptor;
            this.Columns = descriptor.Fields.Select(f => f.Key);
            this.Rows = data.Select(row => {
                object[] newRow = new object[this.columns.Length];

                for (int i = 0; i < newRow.Length; i++) 
                {
                    string field = this.columns[i];

                    newRow[i] = row.ContainsKey(field) ? row[field] : null;
                }

                return newRow;
            });
        }

        public Table(EntityDescriptor descriptor, IEnumerable<object[]> data) 
        {
            if (data == null) return;

            this.EntityDescriptor = descriptor;
            this.Rows = data;
            this.Columns = descriptor.Fields.Keys;
        }

        public Table(Table table) : this(table.EntityDescriptor, table.rows) {}

        public EntityDescriptor EntityDescriptor { get; private set; }

        private List<object[]> rows;
        public IEnumerable<object[]> Rows 
        {
            get => this.rows;
            private set 
            {
                rows = value.ToList();
            }
        }

        private string[] columns;
        public IEnumerable<string> Columns 
        {
            get => this.columns;
            private set { this.columns = value.ToArray(); }
        }

        public object[] this[int index]
        {
            get => this.rows[index];
        }

        public ColumnDescriptor this[string column]
        {
            get => this.GetColumnValues(column);
        }

        private ColumnDescriptor GetColumnValues(string column) 
        {
            Type type = this.EntityDescriptor.Fields[column].Type;

            object[] data = new object[this.rows.Count];

            int columnIndex = this.GetColumnIndex(column);

            for(int i = 0; i < this.rows.Count; i++) 
            {
                data[i] = this.rows[i][columnIndex];
            }

            return new ColumnDescriptor(type, data);
        }

        public int GetColumnIndex(string column) 
        {
            for (int i = 0; i < this.columns.Length; i++) 
            {
                if (this.columns[i] == column) return i;
            }

            return -1;
        }

        public void Add(params object[] columnValues) 
        {
            this.Add(columnValues);
        }

        public void Insert(int index, params object[] columnValues) 
        {
            this.rows.Insert(index, columnValues);
        }

        public void RemoveAt(int index) 
        {
            this.rows.RemoveAt(index);
        }

        public void Remove(object[] row)
        {
            this.rows.Remove(row);
        }

        public IEnumerable<object[]> Select(params string[] columns) 
        {
            int[] indexes = columns.Select(c => this.GetColumnIndex(c)).ToArray();

            return this.Rows.Select(row => {
                object[] newRow = new object[indexes.Length];

                for(int i = 0; i < indexes.Length; i++) 
                {
                    int index = indexes[i];
                    newRow[i] = row[index];
                }

                return newRow;
            });
        }
    }
}
