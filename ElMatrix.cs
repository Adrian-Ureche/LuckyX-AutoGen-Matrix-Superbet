using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuckyX__AutoGen
{
    class ElMatrix
    {
        private string id;
        private string quantity;
        private string length;

        public ElMatrix(string id, string quantity, string length, ref TreeView tvList)
        {
            this.id = id;
            this.quantity = quantity;
            this.length = length;

            TreeNode newNode = new TreeNode(id);
            tvList.Nodes.Add(newNode);
        }

        public string Id { get => id; set => id = value; }
        public string Quantity { get => quantity; set => quantity = value; }
        public string Length { get => length; set => length = value; }
    }
}
