using Stockfish_7._1_CSharp_Port.Types;

namespace Stockfish_7._1_CSharp_Port;

class Program
{
    static void Main(string[] args)
    {
        Bitboard.init();
        
        Bitboard b_from = 0b_00000000_00000000_00000000_00010000_00000000_00000000_00000000_00000000;
        Bitboard b_occupancy = 0b_11111111_00000000_00000000_00010101_00100000_00000000_00000000_11111111;
        Square s_from = Bitboard.lsb(b_from);
        PieceType pt = PieceType.PAWN;
        
        System.Console.WriteLine("b_from: ");
        System.Console.WriteLine(Bitboard.pretty(b_from));
        
        System.Console.WriteLine("b_occupancy: ");
        System.Console.WriteLine(Bitboard.pretty(b_occupancy));

        Bitboard b_attack = Bitboard.attacks_bb(s_from, b_occupancy, pt);
        
        System.Console.WriteLine("b_attack: ");
        System.Console.WriteLine(Bitboard.pretty(b_attack));

        // System.Console.WriteLine("Square: " + s.value);
        //
        //
        // Bitboard edges = ((Bitboard.Rank1BB | Bitboard.Rank8BB) & ~Bitboard.rank_bb(s)) | ((Bitboard.FileABB | Bitboard.FileHBB) & ~Bitboard.file_bb(s));
        // UInt64 mask= Bitboard.sliding_attack(PieceType.BISHOP, s, 0) & ~edges;
        // System.Console.WriteLine("mask: ");
        // System.Console.WriteLine(Bitboard.pretty(mask));
        //
        // b = UInt64.MaxValue;
        // int size = 0;
        // do
        // {
        //     //System.Console.WriteLine("Board: " + b.value);
        //     //System.Console.WriteLine(Bitboard.pretty(b));
        //     size++;
        //     b = (b - mask) & mask;
        // } while (b);
        // System.Console.WriteLine(size);



    }
}