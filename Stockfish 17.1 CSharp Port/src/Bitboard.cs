/*
  Stockfish, a UCI chess playing engine derived from Glaurung 2.1
  Copyright (C) 2004-2025 The Stockfish developers (see AUTHORS file)

  Stockfish is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Stockfish is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using Stockfish_7._1_CSharp_Port.Types;
using File = Stockfish_7._1_CSharp_Port.Types.File;

namespace Stockfish_7._1_CSharp_Port;

public struct Bitboard: IEquatable<Bitboard>
{
    public static readonly Bitboard FileABB = 0x0101010101010101UL;
    public static readonly Bitboard FileBBB = FileABB << 1;
    public static readonly Bitboard FileCBB = FileABB << 2;
    public static readonly Bitboard FileDBB = FileABB << 3;
    public static readonly Bitboard FileEBB = FileABB << 4;
    public static readonly Bitboard FileFBB = FileABB << 5;
    public static readonly Bitboard FileGBB = FileABB << 6;
    public static readonly Bitboard FileHBB = FileABB << 7;

    public static readonly Bitboard Rank1BB = 0xFF;
    public static readonly Bitboard Rank2BB = Rank1BB << (8 * 1);
    public static readonly Bitboard Rank3BB = Rank1BB << (8 * 2);
    public static readonly Bitboard Rank4BB = Rank1BB << (8 * 3);
    public static readonly Bitboard Rank5BB = Rank1BB << (8 * 4);
    public static readonly Bitboard Rank6BB = Rank1BB << (8 * 5);
    public static readonly Bitboard Rank7BB = Rank1BB << (8 * 6);
    public static readonly Bitboard Rank8BB = Rank1BB << (8 * 7);
    
    public static readonly byte[,] SquareDistance = new byte[Square.SQUARE_NB, Square.SQUARE_NB];

    public static readonly Bitboard[,] LineBB = new Bitboard[Square.SQUARE_NB, Square.SQUARE_NB];
    public static readonly Bitboard[,] BetweenBB = new Bitboard[Square.SQUARE_NB, Square.SQUARE_NB];
    public static readonly Bitboard[,] PseudoAttacks = new Bitboard[PieceType.PIECE_TYPE_NB, Square.SQUARE_NB];
    public static readonly Bitboard[,] PawnAttacks = new Bitboard[Color.COLOR_NB, Square.SQUARE_NB];
    
    public static readonly Bitboard[] RookTable = new Bitboard[0x19000];   // To store rook attacks
    public static readonly Bitboard[] BishopTable = new Bitboard[0x1480];  // To store bishop attacks
    
    public static readonly Magic[,] Magics = new Magic[Square.SQUARE_NB, 2];
    
    public readonly UInt64 value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard(UInt64 value) => this.value = value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Bitboard other) => value == other.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj) => obj is Bitboard other && Equals(other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => value.GetHashCode();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Bitboard(UInt64 value) => new Bitboard(value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator UInt64(Bitboard b) => b.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator &(Bitboard b1, Bitboard b2) => b1.value & b2.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator |(Bitboard b1, Bitboard b2) => b1.value | b2.value;
    
    // Overloads of bitwise operators between a Bitboard and a Square for testing
    // whether a given bit is set in a bitboard, and for setting and clearing bits.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator &(Bitboard b, Square s) => b & square_bb(s);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator |(Bitboard b, Square s) => b | square_bb(s);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator ^(Bitboard b, Square s) => b ^ square_bb(s);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator &(Square s, Bitboard b) => b & s;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator |(Square s, Bitboard b) => b | s;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator ^(Square s, Bitboard b) => b ^ s;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard operator -(Bitboard b) => (UInt64)(-(Int64)b.value);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator true(Bitboard b) => b!=0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator false(Bitboard b) => b==0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Bitboard b1, Bitboard b2) => b1.value == b2.value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Bitboard b1, Bitboard b2) => b1.value != b2.value;
    
    //From bitboard.h to Square.cs
    //inline Bitboard operator|(Square s1, Square s2) { return square_bb(s1) | s2; }

    // Counts the number of non-zero bits in a bitboard.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Int32 popcount(Bitboard b)
    {
        return BitOperations.PopCount(b);
    }
    
    // Returns the least significant bit in a non-zero bitboard.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square lsb(Bitboard b)
    {
        Debug.Assert(b!=0);
        return BitOperations.TrailingZeroCount(b);
    }
    
    // Returns the most significant bit in a non-zero bitboard.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square msb(Bitboard b)
    {
        Debug.Assert(b!=0);
        return BitOperations.LeadingZeroCount(b);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard square_bb(Square s) {
        Debug.Assert(Square.is_ok(s));
        return (1UL << s);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool more_than_one(Bitboard b) => (b & (b - 1)) != 0; 
    
    // rank_bb() and file_bb() return a bitboard representing all the squares on
    // the given file or rank.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard rank_bb(Rank r) => Rank1BB << (8 * r); 

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard rank_bb(Square s) => rank_bb(Square.rank_of(s)); 

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard file_bb(File f) => FileABB << f; 

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard file_bb(Square s) => file_bb(Square.file_of(s)); 
    
    // Moves a bitboard one or two steps as specified by the direction D
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard shift(Bitboard b, Direction D) {
        return D == Direction.NORTH         ? b << 8
            : D == Direction.SOUTH         ? b >> 8
            : D == Direction.NORTH + Direction.NORTH ? b << 16
            : D == Direction.SOUTH + Direction.SOUTH ? b >> 16
            : D == Direction.EAST          ? (b & ~FileHBB) << 1
            : D == Direction.WEST          ? (b & ~FileABB) >> 1
            : D == Direction.NORTH_EAST    ? (b & ~FileHBB) << 9
            : D == Direction.NORTH_WEST    ? (b & ~FileABB) << 7
            : D == Direction.SOUTH_EAST    ? (b & ~FileHBB) >> 7
            : D == Direction.SOUTH_WEST    ? (b & ~FileABB) >> 9
            : 0;
    }
    
    // Returns the squares attacked by pawns of the given color
    // from the squares in the given bitboard.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard pawn_attacks_bb(Bitboard b, Color C) {
        return C == Color.WHITE ? shift(b, Direction.NORTH_WEST) | shift(b, Direction.NORTH_EAST)
            : shift(b, Direction.SOUTH_WEST) | shift(b, Direction.SOUTH_EAST);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard pawn_attacks_bb(Color c, Square s) {

        Debug.Assert(Square.is_ok(s));
        return PawnAttacks[c, s];
    }
    
    // Returns a bitboard representing an entire line (from board edge
    // to board edge) that intersects the two given squares. If the given squares
    // are not on a same file/rank/diagonal, the function returns 0. For instance,
    // line_bb(SQ_C4, SQ_F7) will return a bitboard with the A2-G8 diagonal.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard line_bb(Square s1, Square s2) {

        Debug.Assert(Square.is_ok(s1) && Square.is_ok(s2));
        return LineBB[s1, s2];
    }
    
    // Returns a bitboard representing the squares in the semi-open
    // segment between the squares s1 and s2 (excluding s1 but including s2). If the
    // given squares are not on a same file/rank/diagonal, it returns s2. For instance,
    // between_bb(SQ_C4, SQ_F7) will return a bitboard with squares D5, E6 and F7, but
    // between_bb(SQ_E6, SQ_F8) will return a bitboard with the square F8. This trick
    // allows to generate non-king evasion moves faster: the defending piece must either
    // interpose itself to cover the check or capture the checking piece.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard between_bb(Square s1, Square s2) {

        Debug.Assert(Square.is_ok(s1) && Square.is_ok(s2));
        return BetweenBB[s1, s2];
    }
    
    // Returns true if the squares s1, s2 and s3 are aligned either on a
    // straight or on a diagonal line.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool aligned(Square s1, Square s2, Square s3) => (line_bb(s1, s2) & s3)!=0; 
    
    // distance() functions return the distance between x and y, defined as the
    // number of steps for a king in x to reach y.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int distanceFile(Square x, Square y) => Math.Abs(Square.file_of(x) - Square.file_of(y));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int distanceRank(Square x, Square y) => Math.Abs(Square.rank_of(x) - Square.rank_of(y));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int distanceSquare(Square x, Square y) => SquareDistance[x, y];
    
    // Returns the bitboard of target square for the given step
    // from the given square. If the step is off the board, returns empty bitboard.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard safe_destination(Square s, int step) {
        Square to = s + step;
        return Square.is_ok(to) && distanceSquare(s, to) <= 2 ? square_bb(to) : 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int edge_distance(File f) => Math.Min(f, File.FILE_H - f); 
    
    // Returns the pseudo attacks of the given piece type
    // assuming an empty board.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard attacks_bb(Square s, PieceType Pt) {

        Debug.Assert((Pt != PieceType.PAWN) && (Square.is_ok(s)));
        return PseudoAttacks[Pt, s];
    }
    
    //Returns the bitboard of the least significant
    //square of a non-zero bitboard. It is equivalent to square_bb(lsb(bb)).
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard least_significant_square_bb(Bitboard b) {
        Debug.Assert(b!=0);
        return b & -b;
    }
    
    // Finds and clears the least significant bit in a non-zero bitboard.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Square pop_lsb(ref Bitboard b) {
        Debug.Assert(b!=0);
        Square s = lsb(b);
        b &= b - 1;
        return s;
    }
    
    // Returns the attacks by the given piece
    // assuming the board is occupied according to the passed Bitboard.
    // Sliding piece attacks do not continue passed an occupied square.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard attacks_bb(Square s, Bitboard occupied, PieceType Pt) {

        Debug.Assert((Pt != PieceType.PAWN) && (Square.is_ok(s)));
        const int BISHOP = 3;
        const int ROOK = 4;
        const int QUEEN = 5;
        
        switch (Pt)
        {
            case BISHOP :
            case ROOK :
                return Magics[s, Pt - PieceType.BISHOP].attacks_bb(occupied);
            case QUEEN :
                return attacks_bb(s, occupied, PieceType.BISHOP) | attacks_bb(s, occupied, PieceType.ROOK);
            default :
                return PseudoAttacks[Pt, s];
        }
    }
    
    // Returns the attacks by the given piece
    // assuming the board is occupied according to the passed Bitboard.
    // Sliding piece attacks do not continue passed an occupied square.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard attacks_bb(PieceType pt, Square s, Bitboard occupied) {

        Debug.Assert((pt != PieceType.PAWN) && (Square.is_ok(s)));
        const int BISHOP = 3;
        const int ROOK = 4;
        const int QUEEN = 5;
        
        switch (pt)
        {
            case BISHOP :
                return attacks_bb(s, occupied, PieceType.BISHOP);
            case ROOK :
                return attacks_bb(s, occupied, PieceType.ROOK);
            case QUEEN :
                return attacks_bb(s, occupied, PieceType.BISHOP) | attacks_bb(s, occupied, PieceType.ROOK);
            default :
                return PseudoAttacks[pt, s];
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard sliding_attack(PieceType pt, Square sq, Bitboard occupied) {

        Bitboard  attacks             = 0;
        Direction[] RookDirections   = {Direction.NORTH, Direction.SOUTH, Direction.EAST, Direction.WEST};
        Direction[] BishopDirections = {Direction.NORTH_EAST, Direction.SOUTH_EAST, Direction.SOUTH_WEST, Direction.NORTH_WEST};

        foreach(Direction d in (pt == PieceType.ROOK ? RookDirections : BishopDirections))
        {
            Square s = sq;
            while (safe_destination(s, d))
            {
                attacks |= (s += d);
                if (occupied & s)
                {
                    break;
                }
            }
        }

        return attacks;
    }
    
    // Computes all rook and bishop attacks at startup. Magic
    // bitboards are used to look up attacks of sliding pieces. As a reference see
    // https://www.chessprogramming.org/Magic_Bitboards. In particular, here we use
    // the so called "fancy" approach.
    public static void init_magics(PieceType pt, Bitboard[] table, Magic[,] magics) {
        // Optimal PRNG seeds to pick the correct magics in the shortest time
        int[,] seeds= {{8977, 44560, 54343, 38998, 5731, 95205, 104912, 17020},
            {728, 10316, 55013, 32803, 12281, 15100, 16645, 255}};

        Bitboard[] occupancy= new Bitboard[4096];
        int[] epoch = new int[4096];
        int cnt = 0;
        
        Bitboard[] reference = new Bitboard[4096];
        int size = 0;

        for (Square s = Square.SQ_A1; s <= Square.SQ_H8; ++s)
        {
            // Board edges are not considered in the relevant occupancies
            Bitboard edges = ((Rank1BB | Rank8BB) & ~rank_bb(s)) | ((FileABB | FileHBB) & ~file_bb(s));

            // Given a square 's', the mask is the bitboard of sliding attacks from
            // 's' computed on an empty board. The index must be big enough to contain
            // all the attacks for each possible subset of the mask and so is 2 power
            // the number of 1s of the mask. Hence we deduce the size of the shift to
            // apply to the 64 or 32 bits word to get the index.
            ref Magic m = ref magics[s, pt - PieceType.BISHOP];
            m.mask   = sliding_attack(pt, s, 0) & ~edges;
            m.shift = 64 - popcount(m.mask);
            
            // Set the offset for the attacks table of the square. We have individual
            // table sizes for each square with "Fancy Magic Bitboards".
            m.attacks = table;
            m.offset = s == Square.SQ_A1 ? 0 : magics[s - 1, pt - PieceType.BISHOP].offset + size;
            //m.attacks = s == Square.SQ_A1 ? table : magics[s - 1, pt - PieceType.BISHOP].attacks + size;
            size = 0;

            // Use Carry-Rippler trick to enumerate all subsets of masks[s] and
            // store the corresponding sliding attack bitboard in reference[].
            Bitboard b = 0;
            do
            {
                occupancy[size] = b;
                reference[size] = sliding_attack(pt, s, b);
                size++;
                b = (b - m.mask) & m.mask;
            } while (b);
            
            PRNG rng = new PRNG(seeds[1, Square.rank_of(s)]);
            // Find a magic for square 's' picking up an (almost) random number
            // until we find the one that passes the verification test.
            for (int i = 0; i < size;)
            {
                for (m.magic = 0; popcount((m.magic * m.mask) >> 56) < 6;)
                    m.magic = rng.sparse_rand();

                // A good magic must map every possible occupancy to an index that
                // looks up the correct sliding attack in the attacks[s] database.
                // Note that we build up the database for square 's' as a side
                // effect of verifying the magic. Keep track of the attempt count
                // and save it in epoch[], little speed-up trick to avoid resetting
                // m.attacks[] after every failed attempt.
                for (++cnt, i = 0; i < size; ++i)
                {
                    UInt32 idx = m.index(occupancy[i]);

                    if (epoch[idx] < cnt)
                    {
                        epoch[idx]     = cnt;
                        m.attacks[m.offset + idx] = reference[i];
                    }
                    else if (m.attacks[m.offset + idx] != reference[i])
                        break;
                }
            }
        }
    }
    
    //Returns an ASCII representation of a bitboard suitable
    //to be printed to standard output. Useful for debugging.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static String pretty(Bitboard b)
    {
        
        String s = "+---+---+---+---+---+---+---+---+\n";
    
        for (Rank r = Rank.RANK_8; r >= Rank.RANK_1; --r)
        {
            for (File f = File.FILE_A; f <= File.FILE_H; ++f)
                s += b & Square.make_square(f, r) ? "| X " : "|   ";
    
            s += "| " + (1 + r) + "\n+---+---+---+---+---+---+---+---+\n";
        }
        s += "  a   b   c   d   e   f   g   h\n";
    
        return s;
    }

    // Initializes various bitboard tables. It is called at
    // startup and relies on global objects to be already zero-initialized.
    public static void init()
    {
        for (Square s1 = Square.SQ_A1; s1 <= Square.SQ_H8; ++s1)
            for (Square s2 = Square.SQ_A1; s2 <= Square.SQ_H8; ++s2)
                SquareDistance[s1, s2] = (byte)Math.Max(distanceFile(s1, s2), distanceRank(s1, s2));
        
        
        init_magics(PieceType.ROOK, RookTable, Magics);
        init_magics(PieceType.BISHOP, BishopTable, Magics);

        for (Square s1 = Square.SQ_A1; s1 <= Square.SQ_H8; ++s1)
        {
            PawnAttacks[Color.WHITE, s1] = pawn_attacks_bb(square_bb(s1), Color.WHITE);
            PawnAttacks[Color.BLACK, s1] = pawn_attacks_bb(square_bb(s1), Color.BLACK);

            foreach (int step in new int[]{-9, -8, -7, -1, 1, 7, 8, 9})
                PseudoAttacks[PieceType.KING, s1] |= safe_destination(s1, step);

            foreach (int step in new int[] {-17, -15, -10, -6, 6, 10, 15, 17})
                PseudoAttacks[PieceType.KNIGHT, s1] |= safe_destination(s1, step);

            PseudoAttacks[PieceType.QUEEN, s1] = PseudoAttacks[PieceType.BISHOP, s1] = attacks_bb(s1, 0, PieceType.BISHOP);
            PseudoAttacks[PieceType.QUEEN, s1] |= PseudoAttacks[PieceType.ROOK, s1]  = attacks_bb(s1, 0, PieceType.ROOK);

            foreach (PieceType pt in new PieceType[]{PieceType.BISHOP, PieceType.ROOK})
                for (Square s2 = Square.SQ_A1; s2 <= Square.SQ_H8; ++s2)
                {
                    if (PseudoAttacks[pt, s1] & s2)
                    {
                        LineBB[s1, s2] = (attacks_bb(pt, s1, 0) & attacks_bb(pt, s2, 0)) | s1 | s2;
                        BetweenBB[s1, s2] =
                            (attacks_bb(pt, s1, square_bb(s2)) & attacks_bb(pt, s2, square_bb(s1)));
                    }
                    BetweenBB[s1, s2] |= s2;
                }
        }
        
    }
    
}