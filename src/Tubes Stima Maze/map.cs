using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;

namespace Tubes_Stima_Maze{
    public class koor{
        public int X{get;set;}
        public int Y{get;set;}

        public koor(int _x, int _y){
            X = _x;
            Y = _y;
        }

        public koor(koor P){
            X = P.X;
            Y = P.Y;
        }


        public override bool Equals(object obj){
            if(obj == null || GetType()!= obj.GetType()){
                return false;
            }

            koor p = (koor)obj;
            return(X == p.X) && (Y == p.Y);
        }

        public override int GetHashCode(){
            return (X+Y).GetHashCode();
        }
    }


    public class BFSnDFS{

        static int row;
        static int col;
        static koor start;


        static Stack<char> reverseStack(Stack<char> p){
            Stack<char> temp = new Stack<char>();
            while (p.Count != 0) {
                temp.Push(p.Pop());
            }
            return temp;
        }

        static Stack<koor> reverseStack(Stack<koor> p){
            Stack<koor> temp = new Stack<koor>();
            while (p.Count != 0) {
                temp.Push(p.Pop());
            }
            return temp;
        }

        public static Stack<koor> DFS(char[,] map){
            int row = map.GetLength(0);
            int col = map.GetLength(1);

            List<koor> treasureDF = new List<koor>();
            Stack<koor> visitedDFS = new Stack<koor>();
            Stack<koor> availDFS = new Stack<koor>();

            koor start = new koor(0,0);
            for (int i = 0; i < row; i++)
            {
                for(int j = 0;j < col; j++)
                {
                    if (map[i, j] == 'T')
                    {
                        koor end = new koor(i, j);
                        treasureDF.Add(end);
                    }
                    else if (map[i, j] == 'K')
                    {
                        start = new koor(i, j);
                    }
                }
            }

            visitedDFS.Push(start);  
            availDFS.Push(start);
            int treasureCount = treasureDF.Count;
            int count = 0;
            while(availDFS.Count>0){
                koor currKoor = new koor(availDFS.Pop());

                visitedDFS.Push(currKoor);
                if(map[currKoor.X,currKoor.Y] == 'T'){
                    count++;
                    treasureDF.Remove(currKoor);
                    if(count==treasureCount){
                        break;
                    }
                }
                foreach(var i in "DLUR"){
                    koor temp2 = new koor(currKoor);
                    if(i=='R'){
                        if(currKoor.Y < col-1){ //CEK PINGGIRAN
                            if(map[currKoor.X,currKoor.Y+1] != 'X'){
                                temp2 = new koor(currKoor.X,currKoor.Y+1);
                            }
                        }
                    } else if(i=='L'){
                        if(currKoor.Y>0){ //CEK PINGGIRAN
                            if(map[currKoor.X,currKoor.Y-1] != 'X'){
                                temp2 = new koor(currKoor.X,currKoor.Y-1);
                            }
                        }
                    } else if(i=='U'){
                        if(currKoor.X > 0){ //CEK PINGGIRAN
                            if(map[currKoor.X-1,currKoor.Y] != 'X'){
                                temp2 = new koor(currKoor.X - 1, currKoor.Y);
                            }
                        }
                    } else if(i=='D'){
                        if(currKoor.X < row-1){ //CEK PINGGIRAN
                            if(map[currKoor.X+1,currKoor.Y] != 'X'){
                                temp2 = new koor(currKoor.X+1,currKoor.Y);
                            }
                        }
                    }
    
                    if(visitedDFS.Contains(temp2)){
                        continue;
                    }
                    availDFS.Push(temp2);
                }
            }

            return visitedDFS;
        }

        public static Queue<koor> BFS(char[,] map){
            List<koor> treasureBF = new List<koor>();
            Queue<koor> visitedBFS = new Queue<koor>();
            Queue<koor> availBFS = new Queue<koor>();
            int row = map.GetLength(0);
            int col = map.GetLength(1);
            koor start = new koor(0, 0);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (map[i, j] == 'T')
                    {
                        koor end = new koor(i, j);
                        treasureBF.Add(end);
                    }
                    else if (map[i, j] == 'K')
                    {
                        start = new koor(i, j);
                    }
                }
            }
            visitedBFS.Enqueue(start);
            availBFS.Enqueue(start);
            int treasureCount = treasureBF.Count;
            int count = 0;
            while(availBFS.Count>0){
                koor currKoor = new koor(availBFS.Dequeue());
                visitedBFS.Enqueue(currKoor);
                if(map[currKoor.X,currKoor.Y] == 'T'){
                    count++;
                    treasureBF.Remove(currKoor);
                    if(count==treasureCount){
                        break;
                    }
                }
                foreach(var i in "RULD"){
                    koor temp2 = new koor(currKoor);
                    if(i=='R'){
                        if(currKoor.Y < col-1){ //CEK PINGGIRAN
                            if(map[currKoor.X,currKoor.Y+1] != 'X'){
                                temp2 = new koor(currKoor.X,currKoor.Y+1);
                            }
                        }
                    }else if(i=='L'){
                        if(currKoor.Y>0){ //CEK PINGGIRAN
                            if(map[currKoor.X,currKoor.Y-1] != 'X'){
                                temp2 = new koor(currKoor.X,currKoor.Y-1);
                            }
                        }
                    }else if(i=='U'){
                        if(currKoor.X > 0){ //CEK PINGGIRAN
                            if(map[currKoor.X-1,currKoor.Y] != 'X'){
                                temp2 = new koor(currKoor.X - 1, currKoor.Y);
                            }
                        }
                    }else if(i=='D'){
                        if(currKoor.X < row-1){ //CEK PINGGIRAN
                            if(map[currKoor.X+1,currKoor.Y] != 'X'){
                                temp2 = new koor(currKoor.X+1,currKoor.Y);
                            }
                        }
                    }
    
                    if(visitedBFS.Contains(temp2) || availBFS.Contains(temp2)){
                        continue;
                    } else
                    {
                        availBFS.Enqueue(temp2);
                    }
                    
                }
            }

            return visitedBFS;
        }
    }
}