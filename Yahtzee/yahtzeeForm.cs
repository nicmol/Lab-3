using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Example Yahtzee website if you've never played
// https://cardgames.io/yahtzee/

namespace Yahtzee
{
    public partial class yahtzeeForm : Form
    {
        public yahtzeeForm()
        {
            InitializeComponent();
        }

        // you may find these helpful in manipulating the scorecard and in other places in your code
        private const int NONE = -1;
        private const int ONES = 0;
        private const int TWOS = 1;
        private const int THREES = 2;
        private const int FOURS = 3;
        private const int FIVES = 4;
        private const int SIXES = 5;
        private const int THREE_OF_A_KIND = 6;
        private const int FOUR_OF_A_KIND = 7;
        private const int FULL_HOUSE = 8;
        private const int SMALL_STRAIGHT = 9;
        private const int LARGE_STRAIGHT = 10;
        private const int CHANCE = 11;
        private const int YAHTZEE = 12;

        private int rollCount = 0;
        private int uScoreCardCount = 0;

        // you'll need an instance variable for the user's scorecard - an array of 13 ints
        private List<int> userScorecard = new List<int>(13);//nm
        // as well as an instance variable for 0 to 5 dice(can be passed as a parameter in numDie method-nm)as
        //the user rolls - array or list<int>?
        private List<int> userRollsDiceNumbers = new List<int>(5);//nm
        // as well as an instance variable for 0 to 5 dice that the user wants to keep - array or list<int>? 
        private List<int> userKeepsDiceNumbers = new List<int>(5);//nm

        // this is the list of methods that I used

        // START WITH THESE 2
        // This method rolls numDie and puts the results in the list
        public void Roll(int numDie, List<int> dice)
        {         
            Random rnd = new Random();
            for (int i = 0; i< numDie; i++)
            {
                dice.Add(rnd.Next(1, 7));

            }
        }

        // This method moves all of the rolled dice to the keep dice before scoring.  All of the dice that
        // are being scored have to be in the same list 
        public void MoveRollDiceToKeep(List<int> roll, List<int> keep)
        {
            for (int i= 0; i < roll.Count; i++)
            {
                keep.Add(roll[i]);

            }

        }

        #region Scoring Methods
        /* This method returns the number of times the parameter value occurs in the list of dice.
         * Calling it with 5 and the following dice 2, 3, 5, 5, 6 returns 2.
         */
        
        public int Count(int value, List<int> dice)
        {
            
                int count = 0;
            for (int i = 0; i <= dice.Count - 1; i++)
            {

                if (dice[i] == value)
                {
                    count = count + 1;
                }

            }
            return count;
        }


        /* This method counts how many 1s, 2s, 3s ... 6s there are in a list of ints that represent a set of dice
         * It takes a list of ints as it's parameter.  It should create an array of 6 integers to store the counts.
         * It should then call Count with a value of 1 and store the result in the first element of the array.
         * It should then repeat the process of calling Count with 2 - 6.
         * It returns the array of counts.
         * All of the rest of the scoring methods can be "easily" calculated using the array of counts.
         */
        private int[] GetCounts(List<int> dice)

        {
            int[] counts = new int[6];
            for(int i = ONES; i<= SIXES; i++)
            {
                int value = i + 1;

             counts[i] = Count(value, dice);
            }
            return counts;
        }

        /* Each of these methods takes the array of counts as a parameter and returns the score for a dice value.
         */
        private int ScoreOnes(int[] counts)
        {
            int value;
            value = counts[ONES];
            int result = value * 1;
            return result;
        }

        private int ScoreTwos(int[] counts)
        {
            int value;
            value = counts[TWOS];
            int result = value * 2;
            return result;
            
        }

        private int ScoreThrees(int[] counts)
        {
            int value;
            value = counts[THREES];
            int result = value * 3;
            return result;
        }

        private int ScoreFours(int[] counts)
        {
            int value;
            value = counts[FOURS];
            int result = value * 4;
            return result;
        }

        private int ScoreFives(int[] counts)
        {
            int value;
            value = counts[FIVES];
            int result = value * 5;
            return result;
        }

        private int ScoreSixes(int[] counts)
        {
            int value;
            value = counts[SIXES];
            int result = value * 6;
            return result;
        }

        /* This method can be used to determine if you have 3 of a kind (or 4? or  5?).  The output parameter
         * whichValue tells you which dice value is 3 of a kind.
         */ 
        private bool HasCount(int howMany, int[] counts, out int whichValue)
        {
            int index = ONES;
            foreach (int count in counts)
            {
                if (howMany == count)
                {
                    whichValue = index;
                    return true;
                }
                index++; // added -nm
            }
            whichValue = NONE;
            return false;
        }

        /* This method returns the sum of the dice represented in the counts array.
         * The sum is the # of 1s * 1 + the # of 2s * 2 + the number of 3s * 3 etc
         */
        private int Sum(int[] counts)
        {
            int diceSum = 0;
            for (int i = 0; i < counts.Length; i++)
            { 
                diceSum = diceSum + ((i + 1) * counts[i]);                                 
            }
            return diceSum;
        }

        /* This method calls HasCount(3...) and if there are 3 of a kind calls Sum to calculate the score.
         * ~Yahtzee = 50 points~
         */
        private int ScoreThreeOfAKind(int[] counts)
        {
            if (HasCount(3, counts, out int whichValue))
            {
                return Sum(counts);
            }
            return 0;
        }
     
        private int ScoreFourOfAKind(int[] counts)
        {
            if (HasCount(4, counts, out int whichValue))
            {
                return Sum(counts);
            }
            return 0;
        }

        private int ScoreYahtzee(int[] counts)
        {
            if (HasCount(5, counts, out int whichValue))
            {
                return 50;
            }
            return 0;
        }

        /* This method calls HasCount(2 and HasCount(3 to determine if there's a full house.  It calls sum to 
         * calculate the score. ~Full house = 25 points~
         */ 
        private int ScoreFullHouse(int[] counts)
        {
            //counts[ONES] = 3;
            //counts[TWOS] = 0;
            //counts[THREES] = 1;
            //counts[FOURS] = 0;
            //counts[FIVES] = 0;
            //counts[SIXES] = 1;


            bool has3k, hasPair;
            has3k = HasCount(3, counts, out int whichVal3k);
            hasPair = HasCount(2, counts, out int whichValPair);
            if(has3k && hasPair && (whichValPair != whichVal3k))
            {
                return 25;
            }

            return 0;
        }

        private int ScoreSmallStraight(int[] counts)//4 dice in a row ~small straight 30 points~
            
        {

            //counts[ONES] = 1;
            //counts[TWOS] = 1;
            //counts[THREES] = 1;
            //counts[FOURS] = 1;
            //counts[FIVES] = 0;
            //counts[SIXES] = 2;

            int checker = 0;
            for(int i =0; i < counts.Length; i++)
            {
                
                if (counts[i] > 0)
                {
                    checker++;                   
                }
                else checker = 0;
               if (checker >= 4)
                {
                    return 30;
                }               
            }
            return 0;
        }
        
        private int ScoreLargeStraight(int[] counts)//5 dice in a row ~large straight 40 points~
        {
            int checker = 0;
            for (int i = 0; i < counts.Length; i++)
            {

                if (counts[i] > 0)
                {
                    checker++;
                }
                else checker = 0;
                if (checker >= 5)
                {
                    return 40;
                }
            }
            return 0;
        }

        private int ScoreChance(int[] counts)
        {
            return Sum(counts);
        }

        /* This method makes it "easy" to call the "right" scoring method when you click on an element
         * in the user score card on the UI.
         */ 
        private int Score(int whichElement, List<int> dice)
        {
            int[] counts = GetCounts(dice);
            switch (whichElement)
            {
                case ONES:
                    return ScoreOnes(counts);
                case TWOS:
                    return ScoreTwos(counts);
                case THREES:
                    return ScoreThrees(counts);
                case FOURS:
                    return ScoreFours(counts);
                case FIVES:
                    return ScoreFives(counts);
                case SIXES:
                    return ScoreSixes(counts);
                case THREE_OF_A_KIND:
                    return ScoreThreeOfAKind(counts);
                case FOUR_OF_A_KIND:
                    return ScoreFourOfAKind(counts);
                case FULL_HOUSE:
                    return ScoreFullHouse(counts);
                case SMALL_STRAIGHT:
                    return ScoreSmallStraight(counts);
                case LARGE_STRAIGHT:
                    return ScoreLargeStraight(counts);
                case CHANCE:
                    return ScoreChance(counts);
                case YAHTZEE:
                    return ScoreYahtzee(counts);
                default:
                    return 0;
            }
        }
        #endregion

        // set each value to some negative number because 
        // a 0 or a positive number could be an actual score
        private void ResetScoreCard(List<int> scoreCard, int scoreCardCount)
        {
            for (int i = ONES; i <= YAHTZEE; i++)
            {
                scoreCard.Add(NONE);
            }
            scoreCardCount = 0;
        }

        // this set has to do with user's scorecard UI
        private void ResetUserUIScoreCard()
        {
            
            for (int i = ONES; i <= YAHTZEE; i++)
            {
                Label scoreCardElement = (Label)this.scoreCardPanel.Controls["user" + i];
                scoreCardElement.Text = "";
                scoreCardElement.Enabled = true;
            }
            userSum.Text = "";
            userBonus.Text = "";
            userTotalScore.Text = "";
        }

        // this method adds the subtotals as well as the bonus points when the user is done playing
        //add numbers 1-6 to get sum, if sum is 63 or over add bonus in label, then add sum
       // and bonus to everything below to put in total score label
        public void UpdateUserUIScoreCard()
        {
            int bottomTotal = 0;
            int bonus = 0;
            int sum = 0;
            for(int i= ONES; i <= SIXES; i++)
            {
                sum = sum + userScorecard[i];                              
            }
            userSum.Text = sum.ToString();
            if(sum >= 63)
            {
                bonus = 35; 
            }
            userBonus.Text = bonus.ToString();
            for(int i = THREE_OF_A_KIND; i <= YAHTZEE; i++)
            {
                bottomTotal = bottomTotal + userScorecard[i];
            }
            int total = bottomTotal + sum + bonus;
            userTotalScore.Text = total.ToString();
        }

        /* When I move a die from roll to keep, I put a -1 in the spot that the die used to be in.
         * This method gets rid of all of those -1s in the list.
         */
        private void CollapseDice(List<int> dice)
        {
            int numDice = dice.Count;
            for (int count = 0, i = 0; count < numDice; count++)
            {
                if (dice[i] == -1)
                    dice.RemoveAt(i);
                else
                    i++;
            }
        }

        /* When I move a die from roll to keep, I need to know which pb I can use.  It's the first spot with a -1 in it
         */
        public int GetFirstAvailablePB(List<int> dice)
        {
            return dice.IndexOf(-1);
        }

        #region UI Dice Methods
        /* These are all UI methods */
        private PictureBox GetKeepDie(int i)
        {
            PictureBox die = (PictureBox)this.Controls["keep" + i];
            return die;
        }

        public void HideKeepDie(int i)
        {
            GetKeepDie(i).Visible = false;
        }
        public void HideAllKeepDice()
        {
            for (int i = 0; i < 5; i++)
                HideKeepDie(i);
        }

        public void ShowKeepDie(int i)
        {
            PictureBox die = GetKeepDie(i);
            die.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\..\\..\\Dice\\die" + userKeepsDiceNumbers[i] + ".png");
            die.Visible = true;
        }

        public void ShowAllKeepDie()
        {
            for (int i = 0; i < 5; i++)
                ShowKeepDie(i);//nm
        }

        private PictureBox GetComputerKeepDie(int i)
        {
            PictureBox die = (PictureBox)this.Controls["computerKeep" + i];
            return die;
        }

        public void HideComputerKeepDie(int i)
        {
            GetComputerKeepDie(i).Visible = false;
        }

        public void HideAllComputerKeepDice()
        {
            for (int i = 0; i < 5; i++)
                HideComputerKeepDie(i);
        }

        public void ShowComputerKeepDie(int i)
        {
            PictureBox die = GetComputerKeepDie(i);
            //die.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\..\\..\\Dice\\die" + i + ".png");
            die.Visible = true;
        }

        public void ShowAllComputerKeepDie()
        {
            for (int i = 0; i < 5; i++)
                ShowComputerKeepDie(i);//nm
        }

        private PictureBox GetRollDie(int i)
        {
            PictureBox die = (PictureBox)this.Controls["roll" + i];
            return die;
        }

        public void HideRollDie(int i)
        {
            GetRollDie(i).Visible = false;
        }

        public void HideAllRollDice()
        {
            for (int i = 0; i < 5; i++)
                HideRollDie(i);
        }

        public void ShowRollDie(int i)
        {
            PictureBox die = GetRollDie(i);
            die.Image = Image.FromFile(System.Environment.CurrentDirectory + "\\..\\..\\Dice\\die" + userRollsDiceNumbers[i] + ".png");
            die.Visible = true;
        }

        public void ShowAllRollDie()
        {
            for (int i = 0; i < 5; i++)
                ShowRollDie(i);//nm
        }
        #endregion

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            ResetUserUIScoreCard();//reset user's scorecard UI
            ResetScoreCard(userScorecard, uScoreCardCount);// reset the user's scorecard
            HideAllRollDice();// Hide the roll dice             
            HideAllKeepDice(); // Hide the keep dice
            HideAllComputerKeepDice();// Hide the computer's dice
              
        }

        private void rollButton_Click(object sender, EventArgs e)
        {

            // DON'T WORRY ABOUT ROLLING MULTIPLE TIMES UNTIL YOU CAN SCORE ONE ROLL
            HideAllKeepDice();// hide all of the keep picture boxes
                              // any of the die that were moved back and forth from roll to keep by the user
                              // are "collapsed" in the keep data structure
                              // show the keep dice again

            // START HERE
            userRollsDiceNumbers.Clear();// clear the roll data structure
            HideAllRollDice();// hide all of the roll picture boxes

            Roll(5, userRollsDiceNumbers); // roll the right number of dice
            ShowAllRollDie();// show the roll picture boxes

            rollCount++;// increment the number of rolls
            if(rollCount == 3)    // disable the button if you've rolled 3 times
            {
                rollButton.Enabled = false;

            }
        }

        private void userScorecard_DoubleClick(object sender, EventArgs e)
        {
            MoveRollDiceToKeep(userRollsDiceNumbers, userKeepsDiceNumbers);// move any rolled die into the keep dice
            HideAllRollDice();// hide picture boxes for both roll and keep
            HideAllKeepDice();
            // determine which element in the score card was clicked
            Label l = (Label)sender; // Gets the label object from the sender
            string labelName = l.Name;// Gets the name of the label
            string labelNumber = labelName.Substring((labelName.IndexOf("er") + 2));//gets the number substring out of the label name string
            int.TryParse(labelNumber, out int index);//converting the string to an integer and putting it in a new variable            
            
            int score = Score(index, userKeepsDiceNumbers);// score that element
            l.Text = score.ToString();// put the score in the scorecard and the UI
            userScorecard[index] = score;//puts the score in the scorecard
            l.Enabled = false;// disable this element in the score card

            userKeepsDiceNumbers.Clear();// clear the keep dice
            rollCount = 0;// reset the roll count
            uScoreCardCount++;// increment the number of elements in the score card that are full
            rollButton.Enabled = true;// enable/disable buttons

            if(uScoreCardCount== 13)  // when it's the end of the game
            {
                UpdateUserUIScoreCard(); // update the sum(s) and bonus parts of the score card
                                         // update the sum(s) and bonus parts of the score card
                rollButton.Enabled = false; // enable/disable buttons
                MessageBox.Show("Game over!"); // display a message box?
            }        
                      
        }
       

        private void roll_DoubleClick(object sender, EventArgs e)//moving dice to the keep area-nm
        {
            // figure out which die you clicked on

            // figure out where in the set of keep picture boxes there's a "space"
            // move the roll die value from this die to the keep data structure in the "right place"
            // sometimes that will be at the end but if the user is moving dice back and forth
            // it may be in the middle somewhere

            // clear the die in the roll data structure
            // hide the picture box
        }

        private void keep_DoubleClick(object sender, EventArgs e)
        {
            // figure out which die you clicked on

            // figure out where in the set of roll picture boxes there's a "space"
            // move the roll die value from this die to the roll data structure in the "right place"
            // sometimes that will be at the end but if the user is moving dice back and forth
            // it may be in the middle somewhere

            // clear the die in the keep data structure
            // hide the picture box
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {    
            //Does the same thing as Form Load event
            ResetUserUIScoreCard();
            ResetScoreCard(userScorecard, uScoreCardCount);
            HideAllRollDice();            
            HideAllKeepDice(); 
            HideAllComputerKeepDice();
        }
        #endregion

        private void roll2_Click(object sender, EventArgs e)
        {

        }
    }
}
