using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vemno
{
    public partial class Form1 : Form
    {
        // 7 March 2021 MBoyce NEW 6L Created Class Payee, included ID number to later join lists, and first name and last name 
        class Payee
        {
            public int PayeeID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        // 7 March 2021 MBoyce NEW 6L Created Class PayInfo included ID number to later join lists, and Amount, reason AND payeeID 
        class PayInfo
        {
            public int InfoID { get; set; }
            public string Amount { get; set; }
            public string Reason { get; set; }
            public int PayeeID { get; set; }
        }

        // 7 March 2021 MBoyce NEW 2L Created 2 lists
        private List<Payee> payees = new List<Payee>();
        private List<PayInfo> info = new List<PayInfo>();

        private int infoID, payeeID;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnName_Click(object sender, EventArgs e)
        {
            //7 March 2021 NEW 5L Error Handling. Enesures user inputs value 
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                txtOutput.Text = "Please enter the recipient's name.";
                return;
            }

            //7 March 2021 MBoyce NEW 2L Trims spaces before and afer, then splits the string to seperate the first and last name 
            txtName.Text.Trim();
            string[] names = txtName.Text.Split(' ');


            Payee payee = new Payee();

            //7 March 2021 MBoyce NEW 1L Assigned ID number to each name has different ID
            payee.PayeeID = ++payeeID;

            //7 March 2021 MBoyce NEW 1L First name is inserted into index 0
            payee.FirstName = names[0];

            /*7 March 2021 MBoyce NEW Error Handling 4L. If user does not enter last name, 
             * this will pass through the first name and a empty string for last name OR 
             * if user enters middle name (multiple names), this will only allow first / last name. */

            if (names.Count() == 1)
                payee.LastName = string.Empty;
            else
                payee.LastName = names[names.Count()-1];


            //7 March 2021 MBoyce NEW 1L Adds "payee" or (name) to payees list 
            payees.Add(payee);

            txtOutput.Text = "Saved recipient's name.";

        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            //7 March 2021 MBoyce NEW 7L Error Handling, user may not enter reason and payment without entering name 
            if (payees.Count == 0 || string.IsNullOrEmpty(payees[payees.Count - 1].FirstName))
            {
                txtOutput.Text = "Please enter the recipient's name!!!";
                txtAmount.Clear();
                txtReason.Clear();
                return;
            }
            // 7 March 2021 NEW 5L User must enter value for "amount" 
            if (string.IsNullOrEmpty(txtAmount.Text.Trim()))
            {
                txtOutput.Text = "Please enter the amount to send.";
                return;
            }

            // 7 March 2021 NEW 7L User must enter valid dollar amount 
            decimal amount = 0;
            if (decimal.TryParse(txtAmount.Text, out amount) == false)
            {
                txtOutput.Text = "Please enter a valid dollar amount.";
                txtAmount.Clear();
                return;
            }
            
            //7 March 2021 NEW 5L User must enter reason 
            if (string.IsNullOrEmpty(txtReason.Text.Trim()))
            {
                txtOutput.Text = "Please enter the reason you are sending money.";
                return;
            }

            PayInfo payInfo = new PayInfo();

            /*7 March 2021 MBoyce NEW 4L Incrementing infoID, assigning values to Amount and Reason,
             * THEN ensuring the ID for the transaction is the same! Now the name and payment information has SAME ID*/

            payInfo.InfoID = ++infoID;
            payInfo.Amount = txtAmount.Text;
            payInfo.Reason = txtReason.Text;
            payInfo.PayeeID = payees.Last().PayeeID;

            // 7 March 2021 MBoyce NEW 1L Adding payment information to list 
            info.Add(payInfo);

            txtOutput.Text = "Saved payment information...";

            txtName.Clear();
            txtAmount.Clear();
            txtReason.Clear();

            //txtName.ReadOnly = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string search = txtSearch.Text.Trim();

            // 7 March 2021 MBoyce NEW 14L Combined lists, then allowed search through by typing the beginning of the first/last name, amount, or reason. 
            var query = info.Join(
                payees,
                info => info.InfoID,
                payee => payee.PayeeID,
                (info, payee) => new
                {
                    FirstName = payee.FirstName,
                    LastName = payee.LastName,
                    Amount = info.Amount,
                    Reason = info.Reason,
                }).Where(s => s.FirstName == search || 
                              s.LastName == search ||
                              s.Amount == search ||
                              s.Reason == search);

            txtOutput.Clear();

            // 7 March 2021 MBoyce NEW 6L Output 
            foreach (var res in query)
            {
                txtOutput.AppendText("Name: "+ res.FirstName + " " + res.LastName +
                                     " - Amount Sent: " + string.Format("{0:c}", decimal.Parse(res.Amount)) + 
                                     " - Reason: " + res.Reason + Environment.NewLine);
            }

            txtSearch.Clear();
        }

        private void btnStartsW_Click(object sender, EventArgs e)
        {
            string search = txtStarts.Text.Trim();

            // 7 March 2021 MBoyce NEW 14L Combined lists, then allowed search through by typing the beginning of the first/last name, amount, or reason. 
            var query = info.Join(
                payees,
                info => info.InfoID,
                payee => payee.PayeeID,
                (info, payee) => new
                {
                    FirstName = payee.FirstName,
                    LastName = payee.LastName,
                    Amount = info.Amount,
                    Reason = info.Reason,
                }).Where(s => s.FirstName.StartsWith(search) == true ||
                              s.LastName.StartsWith(search) == true ||
                              s.Amount.StartsWith(search) == true ||
                              s.Reason.StartsWith(search) == true);

            txtOutput.Clear();

            // 7 March 2021 MBoyce NEW 6L Output 
            foreach (var res in query)
            {
                txtOutput.AppendText("Name: " + res.FirstName + " " + res.LastName +
                                     " - Amount Sent: " + string.Format("{0:c}", decimal.Parse(res.Amount)) +
                                     " - Reason: " + res.Reason + Environment.NewLine);
            }

            txtStarts.Clear();
        }


        private void btnEndsW_Click(object sender, EventArgs e)
        {
            string search = txtEnds.Text.Trim();
            // 7 March 2021 MBoyce NEW 14L Combined lists, then allowed search through by typing the beginning of the first/last name, amount, or reason. 
            var query = info.Join(
                payees,
                info => info.InfoID,
                payee => payee.PayeeID,
                (info, payee) => new
                {
                    FirstName = payee.FirstName,
                    LastName = payee.LastName,
                    Amount = info.Amount,
                    Reason = info.Reason,
                }).Where(s => s.FirstName.EndsWith(search) == true ||
                              s.LastName.EndsWith(search) == true ||
                              s.Amount.EndsWith(search) == true ||
                              s.Reason.EndsWith(search) == true);

            txtOutput.Clear();


            // 7 March 2021 MBoyce NEW 6L Output 
            foreach (var res in query)
            {
                txtOutput.AppendText("Name: " + res.FirstName + " " + res.LastName +
                                     " - Amount Sent: " + string.Format("{0:c}", decimal.Parse(res.Amount)) +
                                     " - Reason: " + res.Reason + Environment.NewLine);
            }

            txtEnds.Clear();
        }
    
    }
}
