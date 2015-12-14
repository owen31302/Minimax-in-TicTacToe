using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ticTacToe
{
    public partial class Form1 : Form
    {
        string turn;
        int turnIndex;
        public List<Button> mButtons = new List<Button>();


        public Form1()
        {
            InitializeComponent();
            ini();
        }

        private void ini() {
            turn = "O";
            turnIndex = 0;
            mButtons.Add(button1);
            mButtons.Add(button2);
            mButtons.Add(button3);
            mButtons.Add(button4);
            mButtons.Add(button5);
            mButtons.Add(button6);
            mButtons.Add(button7);
            mButtons.Add(button8);
            mButtons.Add(button9);
        }

        private void btn_click(object sender, EventArgs e)
        {
            Button mButton = (Button)sender;
            if (mButton.Text.Equals("")) {
                mButton.Text = turn;

                // AI
                if (turnIndex == 0)
                {
                    label1.Text = "default";
                    //default
                    HelperFunction.firstMove(mButtons);
                    turnIndex ++;
                }
                else {
                    int[] returnArray = HelperFunction.saveCurrentDesk(mButtons);
                    int index = HelperFunction.checkMate(returnArray);
                    if (index != -1)
                    {
                        label1.Text = "checkMate";
                        mButtons[index].Text = "X";
                    }
                    else {
                        if (turnIndex < 4)
                        {
                            label1.Text = "AI";
                            //Minimax
                            desk mdesk = new desk(returnArray);
                            List<desk> possibleSuccessors = HelperFunction.buildtree(mdesk, 2);
                            List<desk> possibleSuccessors2 = new List<desk>();
                            for (int i = 0; i < possibleSuccessors.Count; i++)
                            {
                                possibleSuccessors2.AddRange(HelperFunction.buildtree(possibleSuccessors[i], 1));
                            }
                            // evaluate and minmax
                            for (int i = 0; i < possibleSuccessors2.Count; i++)
                            {
                                HelperFunction.evaluate(possibleSuccessors2[i]);
                                HelperFunction.minmax(possibleSuccessors2[i], possibleSuccessors);
                            }
                            // make decision
                            int maxValue = int.MinValue;
                            List<int> maxValueIndex = new List<int>();
                            for (int i = 0; i < possibleSuccessors.Count; i++)
                            {
                                if (possibleSuccessors[i].value > maxValue)
                                {
                                    maxValue = possibleSuccessors[i].value;
                                }
                            }
                            for (int i = 0; i < possibleSuccessors.Count; i++)
                            {
                                if (possibleSuccessors[i].value == maxValue)
                                {
                                    maxValueIndex.Add(i);
                                }
                            }
                            Random mRandom = new Random();
                            int num = mRandom.Next(0, maxValueIndex.Count);
                            desk desk1 = possibleSuccessors[maxValueIndex[num]];
                            int index2 = HelperFunction.findAddPointPosition(desk1);
                            mButtons[index2].Text = "X";
                        }
                        else {
                            label1.Text = "finish";
                        }
                    }
                    turnIndex++;
                }
            }
        }

        private void clearall(object sender, EventArgs e)
        {
            turnIndex = 0;
            for (int i = 0; i < mButtons.Count; i++) {
                mButtons[i].Text = "";
            }
            label1.Text = "Status";
        }
    }

    public class HelperFunction {
        static public int findAddPointPosition(desk mydesk) {
            int index = -1;
            for (int i = 0; i < mydesk.parent.Length; i++) {
                if (!mydesk.parent[i].Equals(mydesk.self[i])) {
                    index = i;
                    break;
                }
            }
            return index;
        }

        static public int checkMate(int[] returnArray)
        {
            bool flag = false;
            int index = -1;
            //horizontal
            for (int i = 0; i < 3; i++) {
                int index_O = 0;
                int index_X = 0;
                int index_ = -1;
                for (int j = 0; j < 3; j++) {
                    if (returnArray[i*3 + j].Equals(1))
                    {
                        index_O++;
                    }
                    else if (returnArray[i * 3 + j].Equals(2))
                    {
                        index_X++;
                    }
                    else {
                        index_ = i * 3 + j;
                    }
                }
                if (index_X == 2 && index_O == 0)
                {
                    flag = true;
                    index = index_;
                    break;
                }else if (index_O == 2 && index_X == 0) {
                    flag = true;
                    index = index_;
                    break;
                }
            }
            if (!flag) {
                //vertical
                for (int i = 0; i < 3; i++)
                {
                    int index_O = 0;
                    int index_X = 0;
                    int index_ = -1;
                    for (int j = 0; j < 3; j++)
                    {
                        if (returnArray[i  + j*3].Equals(1))
                        {
                            index_O++;
                        }
                        else if (returnArray[i + j * 3].Equals(2))
                        {
                            index_X++;
                        }
                        else {
                            index_ = i  + j * 3;
                        }
                    }
                    if (index_X == 2 && index_O == 0)
                    {
                        flag = true;
                        index = index_;
                        break;
                    }
                    else if (index_O == 2 && index_X == 0)
                    {
                        flag = true;
                        index = index_;
                        break;
                    }
                }
            }
            if (!flag)
            {
                //diagonal(up left to down right)
                int index_O = 0;
                int index_X = 0;
                int index_ = -1;
                for (int i = 0; i < 3; i++) {
                    if (returnArray[i*4].Equals(1))
                    {
                        index_O++;
                    }
                    else if (returnArray[i * 4].Equals(2))
                    {
                        index_X++;
                    }
                    else {
                        index_ = i*4;
                    }
                }
                if (index_X == 2 && index_O == 0)
                {
                    flag = true;
                    index = index_;
                }
                else if (index_O == 2 && index_X == 0)
                {
                    flag = true;
                    index = index_;
                }
                if (!flag) {
                    //diagonal(down left to up right)
                    index_O = 0;
                    index_X = 0;
                    index_ = -1;
                    for (int i = 1; i < 4; i++)
                    {
                        if (returnArray[i * 2].Equals(1))
                        {
                            index_O++;
                        }
                        else if (returnArray[i * 2].Equals(2))
                        {
                            index_X++;
                        }
                        else {
                            index_ = i * 2;
                        }
                    }
                    if (index_X == 2 && index_O == 0)
                    {
                        flag = true;
                        index = index_;
                    }
                    else if (index_O == 2 && index_X == 0)
                    {
                        flag = true;
                        index = index_;
                    }
                }

            }
            return index;
        }

        static public void minmax(desk mydesk, List<desk> parents) {
            for (int i = 0; i < parents.Count; i++) {
                if (parents[i].self.SequenceEqual(mydesk.parent)) {
                    if (mydesk.MinorMax == 1) {
                        parents[i].value = Math.Max(parents[i].value, mydesk.value);
                    }
                    else {
                        parents[i].value = Math.Min(parents[i].value, mydesk.value);
                    }
                    break;
                }
            }
        }

        static public void evaluate(desk mydesk)
        {
            int value = 0;
            int index_O;
            int index_X;
            //horizontal
            for (int i = 0; i < 3; i++)
            {
                index_O = 0;
                index_X = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (mydesk.self[i * 3 + j].Equals(1))
                    {
                        index_O++;
                    }
                    else if (mydesk.self[i * 3 + j].Equals(2))
                    {
                        index_X++;
                    }
                }
                if (index_O == 1 && index_X == 0)
                {
                    value--;
                }
                else if (index_O == 2 && index_X == 0)
                {
                    value -= 2;
                }
                else if (index_X == 1 && index_O == 0)
                {
                    value++;
                }
                else if (index_X == 2 && index_O == 0)
                {
                    value += 2;
                }
            }
            //vertical
            for (int i = 0; i < 3; i++)
            {
                index_O = 0;
                index_X = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (mydesk.self[i + j * 3].Equals(1))
                    {
                        index_O++;
                    }
                    else if (mydesk.self[i + j * 3].Equals(2))
                    {
                        index_X++;
                    }
                }
                if (index_O == 1 && index_X == 0)
                {
                    value--;
                }
                else if (index_O == 2 && index_X == 0)
                {
                    value -= 2;
                }
                else if (index_X == 1 && index_O == 0)
                {
                    value++;
                }
                else if (index_X == 2 && index_O == 0)
                {
                    value += 2;
                }
            }
            //diagonal(up left to down right)
            index_O = 0;
            index_X = 0;
            for (int i = 0; i < 3; i++)
            {
                if (mydesk.self[i * 4].Equals(1))
                {
                    index_O++;
                }
                else if (mydesk.self[i * 4].Equals(2))
                {
                    index_X++;
                }
            }
            if (index_O == 1 && index_X == 0)
            {
                value--;
            }
            else if (index_O == 2 && index_X == 0)
            {
                value -= 2;
            }
            else if (index_X == 1 && index_O == 0)
            {
                value++;
            }
            else if (index_X == 2 && index_O == 0)
            {
                value += 2;
            }
            //diagonal(down left to up right)
            index_O = 0;
            index_X = 0;
            for (int i = 1; i < 4; i++)
            {
                if (mydesk.self[i * 2].Equals(1))
                {
                    index_O++;
                }
                else if (mydesk.self[i * 2].Equals(2))
                {
                    index_X++;
                }
            }
            if (index_O == 1 && index_X == 0)
            {
                value--;
            }
            else if (index_O == 2 && index_X == 0)
            {
                value -= 2;
            }
            else if (index_X == 1 && index_O == 0)
            {
                value++;
            }
            else if (index_X == 2 && index_O == 0)
            {
                value += 2;
            }
            mydesk.value = value;
        }

        static public List<desk> buildtree(desk mydesk, int trun) {
            List<int> possibleNodes = new List<int>();
            List<desk> possibleSuccessors = new List<desk>();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (mydesk.self[i * 3 + j] == 0) {
                        possibleNodes.Add(i * 3 + j);
                    }
                }
            }
            for (int i = 0; i < possibleNodes.Count; i++) {
                int[] copy = new int[mydesk.self.Length];
                Array.Copy(mydesk.self, 0, copy, 0, mydesk.self.Length);
                copy[possibleNodes[i]] = trun;
                desk mdesk = new desk(copy, mydesk.self, trun);
                possibleSuccessors.Add(mdesk);
            }
            return possibleSuccessors;
        }

        static public int[] saveCurrentDesk(List<Button> mButtons) {
            int[] returnArray = new int[9];
            for (int i = 0; i< mButtons.Count; i++) {
                if (mButtons[i].Text.Equals("O")) {
                    returnArray[i] = 1; //human
                } else if (mButtons[i].Text.Equals("X")){
                    returnArray[i] = 2; //AI
                }
            }
            return returnArray;
        }

        static public void firstMove(List<Button> mButtons) {
            int[] returnArray = saveCurrentDesk(mButtons);
            Random mRandom = new Random();
            
            //side
            //center
            //middle

            //side
            if (returnArray.SequenceEqual(new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0 })) {
                int index = mRandom.Next(0, 2);
                if (index == 0)
                {
                    mButtons[4].Text = "X";
                }
                else {
                    mButtons[8].Text = "X";
                }
            } else if (returnArray.SequenceEqual(new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0 }))
            {
                int index = mRandom.Next(0, 2);
                if (index == 0)
                {
                    mButtons[4].Text = "X";
                }
                else {
                    mButtons[6].Text = "X";
                }
            }
            else if (returnArray.SequenceEqual(new int[] { 0, 0, 0, 0, 0, 0, 1, 0, 0 }))
            {
                int index = mRandom.Next(0, 2);
                if (index == 0)
                {
                    mButtons[4].Text = "X";
                }
                else {
                    mButtons[2].Text = "X";
                }
            }
            else if (returnArray.SequenceEqual(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1 }))
            {
                int index = mRandom.Next(0, 2);
                if (index == 0)
                {
                    mButtons[4].Text = "X";
                }
                else {
                    mButtons[0].Text = "X";
                }
            }//center
            else if (returnArray.SequenceEqual(new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0 }))
            {
                int index = mRandom.Next(0, 4);
                if (index == 0)
                {
                    mButtons[0].Text = "X";
                } else if (index == 1) {
                    mButtons[2].Text = "X";
                }
                else if (index == 2)
                {
                    mButtons[6].Text = "X";
                }
                else {
                    mButtons[8].Text = "X";
                }
            }// middle
            else if (returnArray.SequenceEqual(new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0 }))
            {
                int index = mRandom.Next(0, 5);
                switch(index){
                    case 0:
                        //MessageBox.Show("一碗100元");
                        mButtons[7].Text = "X";
                        break;//每個case 要以break;結尾
                    case 1:
                        mButtons[0].Text = "X";
                        break;//每個case 要以break;結尾
                    case 2:
                        mButtons[2].Text = "X";
                        break;//每個case 要以break;結尾
                    case 3:
                        mButtons[6].Text = "X";
                        break;//每個case 要以break;結尾
                    case 4:
                        mButtons[8].Text = "X";
                        break;//每個case 要以break;結尾
                    default://以上都不成立執行預設值
                        break;
                }
            }
            else if (returnArray.SequenceEqual(new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0 }))
            {
                int index = mRandom.Next(0, 5);
                switch (index)
                {
                    case 0:
                        //MessageBox.Show("一碗100元");
                        mButtons[5].Text = "X";
                        break;//每個case 要以break;結尾
                    case 1:
                        mButtons[0].Text = "X";
                        break;//每個case 要以break;結尾
                    case 2:
                        mButtons[6].Text = "X";
                        break;//每個case 要以break;結尾
                    case 3:
                        mButtons[2].Text = "X";
                        break;//每個case 要以break;結尾
                    case 4:
                        mButtons[8].Text = "X";
                        break;//每個case 要以break;結尾
                    default://以上都不成立執行預設值
                        break;
                }
            }
            else if (returnArray.SequenceEqual(new int[] { 0, 0, 0, 0, 0, 1, 0, 0, 0 }))
            {
                int index = mRandom.Next(0, 5);
                switch (index)
                {
                    case 0:
                        //MessageBox.Show("一碗100元");
                        mButtons[3].Text = "X";
                        break;//每個case 要以break;結尾
                    case 1:
                        mButtons[2].Text = "X";
                        break;//每個case 要以break;結尾
                    case 2:
                        mButtons[8].Text = "X";
                        break;//每個case 要以break;結尾
                    case 3:
                        mButtons[0].Text = "X";
                        break;//每個case 要以break;結尾
                    case 4:
                        mButtons[6].Text = "X";
                        break;//每個case 要以break;結尾
                    default://以上都不成立執行預設值
                        break;
                }
            }
            else if (returnArray.SequenceEqual(new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 0 }))
            {
                int index = mRandom.Next(0, 5);
                switch (index)
                {
                    case 0:
                        //MessageBox.Show("一碗100元");
                        mButtons[1].Text = "X";
                        break;//每個case 要以break;結尾
                    case 1:
                        mButtons[6].Text = "X";
                        break;//每個case 要以break;結尾
                    case 2:
                        mButtons[8].Text = "X";
                        break;//每個case 要以break;結尾
                    case 3:
                        mButtons[0].Text = "X";
                        break;//每個case 要以break;結尾
                    case 4:
                        mButtons[2].Text = "X";
                        break;//每個case 要以break;結尾
                    default://以上都不成立執行預設值
                        break;
                }
            }

        }
    }

    public class desk
    {
        public int value;
        public int[] parent;
        public int[] self;
        public int MinorMax;
        public desk()
        {
            this.value = 0;
            this.parent = new int[] {0,0,0,0,0,0,0,0,0 };
            this.self = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            this.MinorMax = 0; //1 for max, -1 for min
        }
        public desk(int[] array)
        {
            this.value = 0;
            this.parent = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            this.self = array;
            this.MinorMax = 0; //2 for max, 1 for min
        }
        public desk(int[] self, int[] parent, int turn)
        {
            this.parent = parent;
            this.self = self;
            this.MinorMax = turn; //2 for max, 1 for min
            if (turn == 1)
            {
                this.value = int.MaxValue;
            }
            else {
                this.value = int.MinValue;
            }
            
        }
    }
}
