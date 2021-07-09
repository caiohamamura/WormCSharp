// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System.Linq;

// namespace worm
// {
//     public enum Direcao
//     {
//         cima,
//         baixo,
//         direita,
//         esquerda
//     }
//     public struct Position
//     {
//         public Position(int x, int y)
//         {
//             this.x = x;
//             this.y = y;
//         }

//         public override bool Equals(Object obj)
//         {
//             Position pos2 = (Position)obj;
//             return (this.x == pos2.x && this.y == pos2.y);
//         }

//         public static bool operator ==(Position pos1, Position pos2)
//         {
//             return pos1.Equals(pos2);
//         }

//         public static bool operator !=(Position pos1, Position pos2)
//         {
//             return !pos1.Equals(pos2);
//         }

//         public override int GetHashCode()
//         {
//             return HashCode.Combine(this.x, this.y);
//         }
//         public int x { get; set; }
//         public int y { get; set; }
//     }
//     public class Worm
//     {
//         const int initialSize = 3;
//         public int tamanho = initialSize;
//         public Position posicaoAtual = new Posicao(15, 10);
//         public Direcao direcao = Direcao.direita;
//         public Direcao direcaoAndada = Direcao.direita;
//         public Queue<Position> corpo = new Queue<Position>();

//         public Worm()
//         {
//             corpo.Enqueue(posicaoAtual);
//             corpo.Enqueue(posicaoAtual);
//             corpo.Enqueue(posicaoAtual);
//         }

//         public bool Anda()
//         {
//             switch (this.direcao)
//             {
//                 case Direcao.cima:
//                     posicaoAtual.y -= 1;
//                     break;
//                 case Direcao.baixo:
//                     posicaoAtual.y += 1;
//                     break;
//                 case Direcao.esquerda:
//                     posicaoAtual.x -= 1;
//                     break;
//                 case Direcao.direita:
//                     posicaoAtual.x += 1;
//                     break;
//             }
//             if (corpo.Count == tamanho)
//                 corpo.Dequeue();
//             foreach (var ponto in corpo)
//             {
//                 if (posicaoAtual == ponto)
//                 {
//                     return false;
//                 }
//             }
//             corpo.Enqueue(posicaoAtual);
//             direcaoAndada = direcao;
//             return true;
//         }

//         public void Come()
//         {
//             tamanho++;
//         }

//         public void TrocaDirecao(Direcao novaDirecao)
//         {
//             switch (novaDirecao)
//             {
//                 case Direcao.cima:
//                     if (direcaoAndada == Direcao.baixo)
//                         return;
//                     break;
//                 case Direcao.esquerda:
//                     if (direcaoAndada == Direcao.direita)
//                         return;
//                     break;
//                 case Direcao.direita:
//                     if (direcaoAndada == Direcao.esquerda)
//                         return;
//                     break;
//                 case Direcao.baixo:
//                     if (direcaoAndada == Direcao.cima)
//                         return;
//                     break;
//             }
//             this.direcao = novaDirecao;

//         }
//     }

//     public class Cenario
//     {
//         const double fatorVertical = 3.0 / 8.0;
//         const int largura = 50;
//         int altura = (int)(largura * fatorVertical);

//         public Worm minhoca = new Minhoca();
//         public Random rand = new Random();
//         public Position comida;

//         public Cenario()
//         {
//             DesenhaCenario();
//             CriaComida();
//         }

//         public void CriaComida()
//         {
//             do
//             {
//                 comida.x = rand.Next(1, largura - 1);
//                 comida.y = rand.Next(1, altura);
//             } while (minhoca.corpo.Any(pos => pos == comida));

//             Console.SetCursorPosition(comida.x, comida.y);
//             Console.ForegroundColor = ConsoleColor.Green;
//             Console.Write("O");
//             Console.ForegroundColor = ConsoleColor.White;
//         }

//         public void DesenhaCenario()
//         {
//             Console.Clear();
//             Console.ForegroundColor = ConsoleColor.White;
//             Console.BackgroundColor = ConsoleColor.Black;
//             Console.WriteLine(new String('-', largura));
//             for (int ii = 0; ii < altura; ii++)
//             {
//                 Console.Write("|");
//                 Console.Write(new String(' ', largura - 2));
//                 Console.WriteLine("|");
//             }
//             Console.WriteLine(new String('-', largura));
//             Console.Write("Pontuação: 0");
//         }

//         void DesenhaMinhoca()
//         {
//             Console.SetCursorPosition(minhoca.posicaoAtual.x, minhoca.posicaoAtual.y);
//             Console.BackgroundColor = ConsoleColor.White;
//             Console.Write("O");
//             Console.BackgroundColor = ConsoleColor.Black;
//         }

//         public void AtualizaPontuacao()
//         {
//             Console.SetCursorPosition(11, altura + 2);
//             Console.Write(minhoca.tamanho - 3);
//         }


//         public void LoopPrincipal()
//         {
//             Console.CursorVisible = false;
//             DesenhaMinhoca();
//             while (true)
//             {
//                 Position rabo = minhoca.corpo.Peek();
//                 if (minhoca.Anda() == false ||
//                     minhoca.posicaoAtual.x < 1 ||
//                     minhoca.posicaoAtual.x > (largura - 2) ||
//                     minhoca.posicaoAtual.y < 1 ||
//                     minhoca.posicaoAtual.y > (altura))
//                 {
//                     goto Morreu;
//                 }
//                 DesenhaMinhoca();
//                 if (minhoca.posicaoAtual == comida)
//                 {
//                     minhoca.Come();
//                     AtualizaPontuacao();
//                     CriaComida();
//                 }
//                 if (rabo != minhoca.posicaoAtual)
//                     Console.SetCursorPosition(rabo.x, rabo.y);
//                 Console.Write(" ");
//                 Console.SetCursorPosition(largura - 1, altura + 2);
//                 Console.Write(" ");
//                 if (minhoca.direcaoAndada == Direcao.cima || minhoca.direcaoAndada == Direcao.baixo)
//                     System.Threading.Thread.Sleep((int)(100 / fatorVertical));
//                 else
//                     System.Threading.Thread.Sleep(100);
//             }
//         Morreu:
//             Console.SetCursorPosition(0, altura + 3);
//             Console.WriteLine("Você morreu!", minhoca.tamanho);
//             Console.ResetColor();
//         }
//     }
//     class Program
//     {
//         static void LerTeclado(Worm minhoca)
//         {
//             while (true)
//             {
//                 var tecla = Console.ReadKey(true);

//                 switch (tecla.Key)
//                 {
//                     case ConsoleKey.UpArrow:
//                         minhoca.TrocaDirecao(Direcao.cima);
//                         break;
//                     case ConsoleKey.DownArrow:
//                         minhoca.TrocaDirecao(Direcao.baixo);
//                         break;
//                     case ConsoleKey.LeftArrow:
//                         minhoca.TrocaDirecao(Direcao.esquerda);
//                         break;
//                     case ConsoleKey.RightArrow:
//                         minhoca.TrocaDirecao(Direcao.direita);
//                         break;
//                     default:
//                         break;
//                 }
//             }
//         }

//         static void limpaCoresCtrlC(object sender, ConsoleCancelEventArgs args) {
//             Console.ResetColor();
//         }
//         static void Main(string[] args)
//         {
//             Console.CancelKeyPress += new ConsoleCancelEventHandler(limpaCoresCtrlC);

//             // Se der erro em algum lugar limpar as cores
//             try
//             {
//                 Cenario cenario = new Cenario();
//                 var task = Task.Run(() =>
//                 {
//                     LerTeclado(cenario.minhoca);
//                 });
//                 cenario.LoopPrincipal();
//             } catch {
//                 Console.ResetColor();
//             }
//         }


//     }
// }
