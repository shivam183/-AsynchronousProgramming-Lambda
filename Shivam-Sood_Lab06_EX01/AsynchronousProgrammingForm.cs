using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Diagnostics;

namespace Shivam_Sood_Lab06_EX01
{
    public partial class AsynchronousProgrammingForm : Form
    {

        delegate bool isEvenOrOdd(int num);

        List<int> intList;
        List<double> doubleList;
        List<char> charList;
        Random rng;

        char currentListType;

        public AsynchronousProgrammingForm()
        {
            InitializeComponent();

            isEvenOrOdd isEven = num => num % 2 == 0;
            isEvenOrOdd isOdd = num => num % 2 != 0;

            checkForEvenOddBtn.Click += (sender, e) =>
              {
                  try
                  {
                      int input = int.Parse(inputNumberTB.Text);

                      if (isEven(input))
                      {
                          MessageBox.Show($"The number {input} is Even", "Even Number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      }
                      if (isOdd(input))
                      {
                          MessageBox.Show($"The number {input} is Odd", "Odd Number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      }
                  }
                  catch(FormatException)
                  {
                      MessageBox.Show($"Please enter an integer into the Input Number TextBox.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
                  finally
                  {
                      inputNumberTB.Text = string.Empty;
                  }
              };

        }

        private BigInteger Factorial (long num)
        {
            if (num == 0)
                return 1;


            return num * Factorial(num - 1);

            
           

        }

        private void AsynchronousProgrammingForm_Load(object sender,EventArgs e)
        {
            calculateBtn.Enabled = false;
            checkForEvenOddBtn.Enabled = false;
            generateValuesBtn.Enabled = false;
            searchBtn.Enabled = false;
            displayBtn.Enabled = false;
            this.searchTB.Enabled = false;
            this.lowIndexTB.Enabled = false;
            this.highIndexTB.Enabled = false;
        }

        private void getFactorialTB_TextChanged(object sender, EventArgs e)
        {
            calculateBtn.Enabled = !getFactorialTB.Text.Equals(string.Empty);
        }
        private void inputNumberTB_TextChanged(object sender, EventArgs e)
        {
           
            checkForEvenOddBtn.Enabled = !inputNumberTB.Text.Equals(String.Empty);
        }

        private void searchTB_TextChanged(object sender, EventArgs e)
        {
           
            searchBtn.Enabled = !searchTB.Text.Equals(String.Empty);
        }

        private void lowIndexTB_TextChanged(object sender, EventArgs e)
        {
            
            displayBtn.Enabled = !(lowIndexTB.Text.Equals(String.Empty) || highIndexTB.Text.Equals(String.Empty));
        }

        private void highIndexTB_TextChanged(object sender, EventArgs e)
        {
          
            displayBtn.Enabled = !(lowIndexTB.Text.Equals(String.Empty) || highIndexTB.Text.Equals(String.Empty));
        }

        private void radioBtn_CheckedChanged(object sender, EventArgs e)
        {
           
            generateValuesBtn.Enabled = intRadioBtn.Checked || doublesRadioBtn.Checked || charRadioBtn.Checked;
        }

        private async void calculateBtn_Click(object sender,EventArgs e)
        {
            try
            {
                int input;
                if((input=int.Parse(getFactorialTB.Text))<0)
                {
                    throw new FormatException();
                }

                lblCalculating.Text = "Calculating";
                BigInteger factorialResult = await Task.Run(() => Factorial(input));
                await Task.Delay(5000);
                lblCalculating.Text = "";
                MessageBox.Show($"The factorial of {getFactorialTB.Text} is {factorialResult:n0}", "Factorial was Successfully Calculated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(FormatException)
            {
                MessageBox.Show($"Please enter a positive integer into the Factorial Input TextBox.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InputValueTooLargeException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.getFactorialTB.Text = string.Empty;
            }
        }

        private void generateValuesBtn_Click(object sender, EventArgs e)
        {
            rng = new Random();
            intList = null;
            doubleList = null;
            charList = null;

            if(intRadioBtn.Checked)
            {
                intList = new List<int>(10);
                for(int i=0;i<intList.Capacity;i++)
                {
                    intList.Add(rng.Next(10, 100));
                }
                listDisplayListBox.DataSource = intList;
                currentListType = 'i';
            }
            else if(doublesRadioBtn.Checked)
            {
                doubleList = new List<double>(10);
                for(int i=0;i<doubleList.Capacity;i++)
                {
                    doubleList.Add(Math.Round(rng.NextDouble() * (100 - 10) + 10, 2));
                }
                listDisplayListBox.DataSource = doubleList;
                currentListType = 'd';
            }
            else
            {
                charList = new List<char>(10);
                for (int i = 0; i < charList.Capacity; i++)
                {
                    charList.Add((char)rng.Next(65, 123)); //generates random char values from ascii 65 (A) to ascii 122 (z)
                }
                this.listDisplayListBox.DataSource = charList;
                currentListType = 'c';
            }
        }
        private void searchBtn_Click(object sender, EventArgs e)
        {
            bool serachValueFound;

            try
            {
                if(currentListType=='i')
                {
                    int searchInputValue = int.Parse(searchTB.Text);
                    serachValueFound = SearchData(intList, searchInputValue);
                }
                else if (currentListType == 'd')
                {
                    double searchInputValue = double.Parse(this.searchTB.Text);
                    serachValueFound = SearchData(doubleList, searchInputValue);
                }
                else 
                {
                    char searchInputValue = char.Parse(this.searchTB.Text);
                    serachValueFound = SearchData(charList, searchInputValue);
                }

                MessageBox.Show($"The search value {this.searchTB.Text} was " + (serachValueFound ? "" : "not ") + "found in the list.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(FormatException)
            {
                MessageBox.Show("Please enter a(n)" +
                    (currentListType == 'i'?"integer":currentListType=='d'?"double":"character")
                    +" value into the Search Input Textbox.","Invalid Input",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                searchTB.Text = string.Empty;
            }
        }


        private bool SearchData<T>(List<T> list,T searchValue)
        {
            return list.Contains(searchValue);
        }
        private void listDisplayListBox_DataSourceChanged(object sender, EventArgs e)
        {
            if (listDisplayListBox.DataSource != null)
            {
                searchTB.Enabled = true;
                lowIndexTB.Enabled = true;
                highIndexTB.Enabled = true;
            }
        }

        private void displayBtn_Click(object sender,EventArgs e)
        {
            try
            {
                int lowIndex = int.Parse(lowIndexTB.Text);
                int highIndex = int.Parse(highIndexTB.Text);

                if(lowIndex<0||lowIndex>9||highIndex<0||highIndex>9)
                {
                    throw new FormatException();
                }

                if(highIndex<lowIndex)
                {
                    throw new IndexOutOfRangeException("High Index must be greater than or equal to Low Index");
                }

                switch(currentListType)
                {
                    case 'i':
                        displayListBox.DataSource = PrintData(intList, lowIndex, highIndex);
                        break;
                    case 'd':
                        displayListBox.DataSource = PrintData(doubleList, lowIndex, highIndex);
                        break;
                    default:
                        displayListBox.DataSource = PrintData(charList, lowIndex, highIndex);
                        break;

                }

            }
            catch(FormatException)
            {
                MessageBox.Show("Please enter positive integers between 0 and 9 into the Index Input TextBoxes.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
          
        }
        private List<T> PrintData<T>(List<T> list,int lowIndex,int highIndex)
        {
            return list.GetRange(lowIndex, highIndex - lowIndex + 1);
        }
    }
    
}
