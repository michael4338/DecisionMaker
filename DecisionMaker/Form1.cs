using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionMaker
{
    public partial class Form1 : Form
    {
        static DecisionMaker decisionMaker = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (decisionMaker == null)
            {
                decisionMaker = new DecisionMaker();
            }

            decisionMaker.addDecision("Buy-A-Car");

            // Basic testing
            Decision decision1 = decisionMaker.GetDecisionByName("Buy-A-Car");
            decision1.addCriteria("InExpensive", 100);
            decision1.addCriteria("Brand", 500);
            decision1.addCriteria("Service", 50);

            Option option1 = decision1.addOption("BMW");
            Option option2 = decision1.addOption("Mercedes Benz");
            Option option3 = decision1.addOption("Toyota");

            option1.SetCriteriaRatingAndValue("InExpensive", 20, "55000");
            option2.SetCriteriaRatingAndValue("InExpensive", 20, "56000");
            option3.SetCriteriaRatingAndValue("InExpensive", 100, "24000");

            option1.SetCriteriaRatingAndValue("Brand", 100, "Nice");
            option2.SetCriteriaRatingAndValue("Brand", 100, "Excellent");
            option3.SetCriteriaRatingAndValue("Brand", 30, "Soso");

            option1.SetCriteriaRatingAndValue("Service", 80, "Good");
            option2.SetCriteriaRatingAndValue("Service", 60, "Bad");
            option3.SetCriteriaRatingAndValue("Service", 90, "Great");

            decision1.ValidateResults(56000, 55000, 29500);
            MessageBox.Show("Buy-A-Car points: \n" + decision1.CalculateAndGetResults());

            // Add new decision
            decisionMaker.addDecision("Buy-A-House");
            Decision decision2 = decisionMaker.GetDecisionByName("Buy-A-House");

            decision2.addCriteria("Location", 500);
            decision2.addCriteria("Condition", 200);
            decision2.addCriteria("Signed School", 100);
            decision2.addCriteria("Price", 500);

            Option option21 = decision2.addOption("MountainView Apmt");
            Option option22 = decision2.addOption("SanJose SFH");
            Option option23 = decision2.addOption("Dublin TownHouse");

            option21.SetCriteriaRatingAndValue("Location", 200, "Close to nice north");
            option22.SetCriteriaRatingAndValue("Location", 100, "Not great");
            option23.SetCriteriaRatingAndValue("Location", 60, "Too far away");

            option21.SetCriteriaRatingAndValue("Condition", 20, "Poor");
            option22.SetCriteriaRatingAndValue("Condition", 100, "Neat");
            option23.SetCriteriaRatingAndValue("Condition", 200, "Great");

            option21.SetCriteriaRatingAndValue("Signed School", 90, "9P");
            option22.SetCriteriaRatingAndValue("Signed School", 20, "5P");
            option23.SetCriteriaRatingAndValue("Signed School", 100, "10P");

            option21.SetCriteriaRatingAndValue("Price", 20, "Expensive");
            option22.SetCriteriaRatingAndValue("Price", 100, "Affordable");
            option23.SetCriteriaRatingAndValue("Price", 120, "Better");

            decision2.ValidateResults(123000, 122000, 140000);
            MessageBox.Show("Buy-A-House points: \n" + decision2.CalculateAndGetResults());

            // Test update&delete options
            option21.OptionName = @"MountainView Condo";
            //decision2.updateOption("MountainView Apmt", option21);
            MessageBox.Show("Update option name: \n" + decision2.CalculateAndGetResults());

            decision2.deleteOption("MountainView Condo");
            decision2.ValidateResults(122000, 140000);
            MessageBox.Show("Delete option: \n" + decision2.CalculateAndGetResults());

            // Test update&delete criteria
            Criteria newCriteria1 = new Criteria("How is the school", 10);
            decision2.updateCriteria("Signed School", newCriteria1);
            decision2.ValidateResults(120200, 131000);
            MessageBox.Show("Update Criteria name and weight: \n" + decision2.CalculateAndGetResults());

            decision2.deleteCriteria("How is the school");
            decision2.ValidateResults(120000, 130000);
            MessageBox.Show("Delete criteria: \n" + decision2.CalculateAndGetResults());

            // Test add options
            Option option24 = decision2.addOption("Sunnyvale Townhouse");
            option24.SetCriteriaRatingAndValue("Location", 100, "Neat");
            // option24.SetCriteriaRatingAndValue("Price", 80, "Bad");
            option24.SetCriteriaRatingAndValue("Condition", 50, "Soso");
            decision2.ValidateResults(120000, 130000, 60000);
            MessageBox.Show("Add an option: \n" + decision2.CalculateAndGetResults());

            // Test add criteria
            decision2.addCriteria("View", 300);
            decision2.ValidateResults(120000, 130000, 60000);
            MessageBox.Show("Add a criteria with all rating as 0: \n" + decision2.CalculateAndGetResults());

            decision2.GetOptionByName("SanJose SFH").SetCriteriaRatingAndValue("View", 200, "mountain view");
            decision2.GetOptionByName("Dublin TownHouse").SetCriteriaRatingAndValue("View", 300, "Beautiful mountain view");
            decision2.GetOptionByName("Sunnyvale Townhouse").SetCriteriaRatingAndValue("View", 30, "Neighbour view");
            decision2.ValidateResults(180000, 220000, 69000);
            MessageBox.Show("Add a criteria with setting ratings to options: \n" + decision2.CalculateAndGetResults());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
