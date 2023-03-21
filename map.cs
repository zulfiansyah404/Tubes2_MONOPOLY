using System;
using System.IO;
using System.Collections.Generic;


namespace ppp{
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


    class Program{

        static int row;
        static int col;
        static koor start;
        static List<koor> treasure = new List<koor>();
        static Stack<koor> visited = new Stack<koor>();
        static Stack<koor> avail = new Stack<koor>();
        static Stack<char> path = new Stack<char>();
        static Stack<koor> pathKoor = new Stack<koor>();

        static char[,] readMap(string fileName){
            string[] readText = File.ReadAllLines(fileName);
            for(int i=0;i<readText.Length;i++){
                readText[i] = readText[i].Replace(" ","");
            }
            row = readText.Length;
            col = readText[0].Length;
            koor end;
            char[,] map = new char[row,col];
            for(int i=0;i<row;i++){
                for(int j=0;j<col;j++){
                    if(readText[i][j] != ' '){
                        map[i,j] = readText[i][j];
                    }
                    if(readText[i][j] == 'K'){
                        start = new koor(i,j);
                    }
                    if(readText[i][j] == 'T'){
                        end = new koor(i,j);
                        treasure.Add(end);
                    }
                }
            }
            return map;
        }

        static void printMap(char[,] map){
            for(int i=0;i<row;i++){
                for(int j=0;j<col;j++){
                    Console.Write(map[i,j]);
                }
                Console.WriteLine();
            }
        }

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

        static bool DFS(char[,] map){
            visited.Push(start);
            avail.Push(start);
            // path.Push(start);
            int treasureCount = treasure.Count;
            int count = 0;
            while(avail.Count>0){
                koor currKoor = new koor(avail.Pop());
                koor tempVisited = new koor(visited.Peek());
                pathKoor.Push(currKoor);
                visited.Push(currKoor);
                // koor treasureTemp = new koor(treasure[0]);
                // Console.WriteLine("VISITED: {0},{1}",tempVisited.X,tempVisited.Y);
                // Console.WriteLine("Treasure: {0},{1}",treasureTemp.X,treasureTemp.Y);
                
                // for(int o=0;o<treasure.Count;o++){
                //     Console.WriteLine("Treasure: {0},{1}",treasure[o].X,treasure[o].Y);
                // }
                Console.WriteLine("Current\t: {0},{1}",currKoor.X,currKoor.Y);
                if(map[currKoor.X,currKoor.Y] == 'T'){
                    Console.WriteLine("FOUND {0}",count+1);
                    count++;
                    treasure.Remove(currKoor);
                    if(count==treasureCount){
                        break;
                    }
                }
                foreach(var i in "RULD"){
                    koor temp2 = new koor(currKoor);
                    // char p = 'O';
                    if(i=='R'){
                        if(currKoor.Y < col-1){ //CEK PINGGIRAN
                            if(map[currKoor.X,currKoor.Y+1] != 'X' /*|| map[currKoor.X,currKoor.Y+1] == 'T' || map[currKoor.X,currKoor.Y+1] == 'K'*/){
                                temp2 = new koor(currKoor.X,currKoor.Y+1);
                                // p = 'R';
                            }
                        }
                    }else if(i=='L'){
                        if(currKoor.Y>0){ //CEK PINGGIRAN
                            if(map[currKoor.X,currKoor.Y-1] != 'X' /*||  map[currKoor.X,currKoor.Y-1] == 'T' || map[currKoor.X,currKoor.Y-1] == 'K'*/){
                                temp2 = new koor(currKoor.X,currKoor.Y-1);
                                // p = 'L';
                            }
                        }
                    }else if(i=='U'){
                        if(currKoor.X > 0){ //CEK PINGGIRAN
                            if(map[currKoor.X-1,currKoor.Y] != 'X' /*|| map[currKoor.X-1,currKoor.Y] == 'T' || map[currKoor.X-1,currKoor.Y] == 'K'*/){
                                temp2 = new koor(currKoor.X-1,currKoor.Y);
                                // p = 'U';
                            }
                        }
                    }else if(i=='D'){
                        if(currKoor.X < row-1){ //CEK PINGGIRAN
                            if(map[currKoor.X+1,currKoor.Y] != 'X' /*|| map[currKoor.X+1,currKoor.Y] == 'T' || map[currKoor.X+1,currKoor.Y] == 'K'*/){
                                temp2 = new koor(currKoor.X+1,currKoor.Y);
                                // p = 'D';
                            }
                        }
                    }
    
                    if(visited.Contains(temp2)){
                        Console.WriteLine("-------------");
                        // path.Pop();
                        continue;
                    }
                    avail.Push(temp2);
                    // visited.Push(temp2);
                }
            }

            return true;
        }

        static void Main(){
            char[,] map = readMap("pp.txt");
            printMap(map);
            Console.WriteLine("Start: {0},{1}",start.X,start.Y);
            // avail.Push(start);
            // Console.WriteLine(avail.Count);
            // koor p = new koor(avail.Pop());
            // Console.WriteLine("Ssss : {0}{1}",p.X,p.Y);
            // Console.WriteLine(p);
            // temp = (int[])treasure.Peek();
            // Console.WriteLine("Treasure: {0},{1}",temp[0],temp[1]);
            // visited.Push(new int[]{0,1});
            // // Console.WriteLine("Visiteddddddd: {0},{1}",((int[])visited.Peek())[0],((int[])visited.Peek())[1]);

            DFS(map);
            // koor temp;
            // while(avail.Count > 0){
            //     temp = new koor(avail.Pop());
            //     Console.WriteLine("Available: {0},{1}",temp.X,temp.Y);
            // }
            // while(visited.Count > 0){
            //     temp = new koor(visited.Pop());
            //     Console.WriteLine("Visited: {0},{1}",temp.X,temp.Y);
            // }
            while(pathKoor.Count > 0){
                koor temp = new koor(pathKoor.Pop());
                koor temp2;
                if(pathKoor.Count == 1){
                    temp2 = new koor(pathKoor.Pop());
                }else{
                    temp2 = new koor(pathKoor.Peek());
                }
                
                if(temp.X == temp2.X){
                    if(temp.Y > temp2.Y){
                        path.Push('R');
                    }else{
                        path.Push('L');
                    }
                }
                else if(temp.Y == temp2.Y){
                    if(temp.X > temp2.X){
                        path.Push('D');
                    }else{
                        path.Push('U');
                    }
                }
            }

            // path = reverseStack(path);
            while(path.Count>0){
                Console.Write(path.Pop());
            }


        }
    }
}