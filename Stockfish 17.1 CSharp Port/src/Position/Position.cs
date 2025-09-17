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
using System.Runtime.CompilerServices;
using Stockfish_17_1_CSharp_Port.Types;

namespace Stockfish_17_1_CSharp_Port.Position;

// Position class stores information regarding the board representation as
// pieces, side to move, hash keys, castling info, etc. Important methods are
// do_move() and undo_move(), used by the search to update node info when
// traversing the search tree.
public class Position : ICloneable 
{
    // Data members
    public Piece[] board = new Piece[Square.SQUARE_NB];
    public Bitboard[] byTypeBB = new Bitboard[PieceType.PIECE_TYPE_NB];
    public Bitboard[] byColorBB = new Bitboard[Color.COLOR_NB];
    public int[] pieceCount = new int[Piece.PIECE_NB];
    public int[] castlingRightsMask = new int[Square.SQUARE_NB];
    public Square[] castlingRookSquare = new Square[CastlingRights.CASTLING_RIGHT_NB];
    public Bitboard[] castlingPath = new Bitboard[CastlingRights.CASTLING_RIGHT_NB];
    public StateInfo st;
    public int gamePly;
    public Color sideToMove;
    public bool chess960;
        
    public static void init()
    {
        
    }
    
    public object Clone() => throw new NotSupportedException("Position no es clonable/copiable");

    // FEN string input/output
    public Position set(String fenStr, bool isChess960, StateInfo si){
        return this;
    }

    public Position set(String code, Color c, StateInfo si)
    {
        return this;
    }

    public String fen()
    {
        return "";
    }
    
    // Position representation
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces(params PieceType[] pts)
    {
        Bitboard result = 0;
        foreach (PieceType pt in pts)
        {
            result |= byTypeBB[pt];
        }
        
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces()
    {
        return pieces(PieceType.ALL_PIECES);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces(Color c){ return byColorBB[c]; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pieces(Color c, params PieceType[] pts)
    {
        return pieces(c) & pieces(pts);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Piece piece_on(Square s)
    {
        Debug.Assert(Square.is_ok(s));
        return board[s];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square ep_square() { return st.epSquare; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Color side_to_move(){ return sideToMove; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool empty(Square s) { return piece_on(s) == Piece.NO_PIECE; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Piece moved_piece(Move m){ return piece_on(m.from_sq()); }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int count(Color c, PieceType Pt){
        return pieceCount[Piece.make_piece(c, Pt)];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int count(PieceType Pt){
        return count(Color.WHITE, Pt) + count(Color.BLACK, Pt);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square square(Color c, PieceType Pt){
        Debug.Assert(count(c, Pt) == 1);
        return Bitboard.lsb(pieces(c, Pt));
    }
    
    // Castling
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool can_castle(CastlingRights cr){ return (st.castlingRights & (int)cr)!=0; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CastlingRights castling_rights(Color c){
        return c & (CastlingRights)st.castlingRights;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool castling_impeded(CastlingRights cr){
        Debug.Assert(cr == CastlingRights.WHITE_OO || cr == CastlingRights.WHITE_OOO || cr == CastlingRights.BLACK_OO || cr == CastlingRights.BLACK_OOO);
        return (pieces() & castlingPath[cr])!=0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Square castling_rook_square(CastlingRights cr){
        Debug.Assert(cr == CastlingRights.WHITE_OO || cr == CastlingRights.WHITE_OOO || cr == CastlingRights.BLACK_OO || cr == CastlingRights.BLACK_OOO);
        return castlingRookSquare[cr];
    }
    
    // Checking
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard checkers(){ return st.checkersBB; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard blockers_for_king(Color c) { return st.blockersForKing[c]; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard check_squares(PieceType pt) { return st.checkSquares[pt]; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard pinners(Color c) { return st.pinners[c]; }
    
    // Attacks to/from a given square
    
    // Computes a bitboard of all pieces which attack a given square.
    // Slider attacks use the occupied bitboard to indicate occupancy.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard attackers_to(Square s, Bitboard occupied) {

        return (Bitboard.attacks_bb(s, occupied, PieceType.ROOK) & pieces(PieceType.ROOK, PieceType.QUEEN))
               | (Bitboard.attacks_bb(s, occupied, PieceType.BISHOP) & pieces(PieceType.BISHOP, PieceType.QUEEN))
               | (Bitboard.pawn_attacks_bb(Color.BLACK, s) & pieces(Color.WHITE, PieceType.PAWN))
               | (Bitboard.pawn_attacks_bb(Color.WHITE, s) & pieces(Color.BLACK, PieceType.PAWN))
               | (Bitboard.attacks_bb(s, PieceType.KNIGHT) & pieces(PieceType.KNIGHT)) | (Bitboard.attacks_bb(s, PieceType.KING) & pieces(PieceType.KING));
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard attackers_to(Square s) { return attackers_to(s, pieces()); }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool attackers_to_exist(Square s, Bitboard occupied, Color c) {

        return (((Bitboard.attacks_bb(s, PieceType.ROOK) & pieces(c, PieceType.ROOK, PieceType.QUEEN))
                && (Bitboard.attacks_bb(s, occupied, PieceType.ROOK) & pieces(c, PieceType.ROOK, PieceType.QUEEN)))
               || ((Bitboard.attacks_bb(s, PieceType.BISHOP) & pieces(c, PieceType.BISHOP, PieceType.QUEEN))
                   && (Bitboard.attacks_bb(s, occupied, PieceType.BISHOP) & pieces(c, PieceType.BISHOP, PieceType.QUEEN)))
               || (((Bitboard.pawn_attacks_bb(~c, s) & pieces(PieceType.PAWN)) | (Bitboard.attacks_bb(s, PieceType.KNIGHT) & pieces(PieceType.KNIGHT))
                                                                               | (Bitboard.attacks_bb(s, PieceType.KING) & pieces(PieceType.KING)))
                   & pieces(c)))!=0;
    }
        
        
    // Calculates st->blockersForKing[c] and st->pinners[~c],
    // which store respectively the pieces preventing king of color c from being in check
    // and the slider pieces of color ~c pinning pieces of color c to the king.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void update_slider_blockers(Color c) {

        Square ksq = square(c, PieceType.KING);

        st.blockersForKing[c] = 0;
        st.pinners[~c]        = 0;

        // Snipers are sliders that attack 's' when a piece and other snipers are removed
        Bitboard snipers = ((Bitboard.attacks_bb(ksq, PieceType.ROOK) & pieces(PieceType.QUEEN, PieceType.ROOK))
                            | (Bitboard.attacks_bb(ksq, PieceType.BISHOP) & pieces(PieceType.QUEEN, PieceType.BISHOP)))
                           & pieces(~c);
        Bitboard occupancy = pieces() ^ snipers;

        while (snipers)
        {
            Square   sniperSq = Bitboard.pop_lsb(ref snipers);
            Bitboard b        = Bitboard.between_bb(ksq, sniperSq) & occupancy;

            if (b!=0 && !Bitboard.more_than_one(b))
            {
                st.blockersForKing[c] |= b;
                if (b & pieces(c))
                    st.pinners[~c] |= sniperSq;
            }
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bitboard attacks_by(Color c, PieceType Pt) {

        if(Pt == PieceType.PAWN)
            return c == Color.WHITE ? Bitboard.pawn_attacks_bb(pieces(Color.WHITE, PieceType.PAWN), Color.WHITE)
            : Bitboard.pawn_attacks_bb(pieces(Color.BLACK, PieceType.PAWN), Color.BLACK);
        else
        {
            Bitboard threats   = 0;
            Bitboard attackers = pieces(c, Pt);
            while (attackers)
                threats |= Bitboard.attacks_bb(Bitboard.pop_lsb(ref attackers), pieces(), Pt);
            return threats;
        }
    }
    
    // Properties of moves
    // bool  legal(Move m) const;
    // bool  pseudo_legal(const Move m) const;
    // bool  capture(Move m) const;
    // bool  capture_stage(Move m) const;
    // bool  gives_check(Move m) const;
    // Piece moved_piece(Move m) const;
    // Piece captured_piece() const;
}