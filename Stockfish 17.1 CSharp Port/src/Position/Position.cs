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
using System.Linq;


namespace Stockfish_17_1_CSharp_Port.Position;

// Position class stores information regarding the board representation as
// pieces, side to move, hash keys, castling info, etc. Important methods are
// do_move() and undo_move(), used by the search to update node info when
// traversing the search tree.
public class Position : ICloneable 
{
    
    public static readonly Piece[] Pieces = {Piece.W_PAWN, Piece.W_KNIGHT, Piece.W_BISHOP, Piece.W_ROOK, Piece.W_QUEEN, Piece.W_KING,
        Piece.B_PAWN, Piece.B_KNIGHT, Piece.B_BISHOP, Piece.B_ROOK, Piece.B_QUEEN, Piece.B_KING};

    public static readonly Color[] Colors = {Color.WHITE, Color.BLACK };
        
    public static readonly CastlingRights[][] Castlingrights = new CastlingRights[][]
    {
        new CastlingRights[] { Color.WHITE & CastlingRights.KING_SIDE,  Color.WHITE & CastlingRights.QUEEN_SIDE },
        new CastlingRights[] { Color.BLACK & CastlingRights.KING_SIDE,  Color.BLACK & CastlingRights.QUEEN_SIDE }
    };

    

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
    // Tests whether a pseudo-legal move is legal
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool legal(Move m) {

        Debug.Assert(m.is_ok());

        Color  us   = sideToMove;
        Square from = m.from_sq();
        Square to   = m.to_sq();

        Debug.Assert(Color.color_of(moved_piece(m)) == us);
        Debug.Assert(piece_on(square(us, PieceType.KING)) == Piece.make_piece(us, PieceType.KING));

        // En passant captures are a tricky special case. Because they are rather
        // uncommon, we do it simply by testing whether the king is attacked after
        // the move is made.
        if (m.type_of() == MoveType.EN_PASSANT)
        {
            Square   ksq      = square(us, PieceType.KING);
            Square   capsq    = to - Direction.pawn_push(us);
            Bitboard occupied = (pieces() ^ from ^ capsq) | to;

            Debug.Assert(to == ep_square());
            Debug.Assert(moved_piece(m) == Piece.make_piece(us, PieceType.PAWN));
            Debug.Assert(piece_on(capsq) == Piece.make_piece(~us, PieceType.PAWN));
            Debug.Assert(piece_on(to) == Piece.NO_PIECE);

            return 0==(Bitboard.attacks_bb(ksq, occupied, PieceType.ROOK) & pieces(~us, PieceType.QUEEN, PieceType.ROOK))
                && 0==(Bitboard.attacks_bb(ksq, occupied, PieceType.BISHOP) & pieces(~us, PieceType.QUEEN, PieceType.BISHOP));
        }

        // Castling moves generation does not check if the castling path is clear of
        // enemy attacks, it is delayed at a later time: now!
        if (m.type_of() == MoveType.CASTLING)
        {
            // After castling, the rook and king final positions are the same in
            // Chess960 as they would be in standard chess.
            to             = Square.relative_square(us, to > from ? Square.SQ_G1 : Square.SQ_C1);
            Direction step = to > from ? Direction.WEST : Direction.EAST;

            for (Square s = to; s != from; s += step)
                if (attackers_to_exist(s, pieces(), ~us))
                    return false;

            // In case of Chess960, verify if the Rook blocks some checks.
            // For instance an enemy queen in SQ_A1 when castling rook is in SQ_B1.
            return !chess960 || 0==(blockers_for_king(us) & m.to_sq());
        }

        // If the moving piece is a king, check whether the destination square is
        // attacked by the opponent.
        if (PieceType.type_of(piece_on(from)) == PieceType.KING)
            return !(attackers_to_exist(to, pieces() ^ from, ~us));

        // A non-king move is legal if and only if it is not pinned or it
        // is moving along the ray towards or away from the king.
        return 0==(blockers_for_king(us) & from) || 0!=(Bitboard.line_bb(from, to) & pieces(us, PieceType.KING));
    }
    
    // Takes a random move and tests whether the move is
    // pseudo-legal. It is used to validate moves from TT that can be corrupted
    // due to SMP concurrent access or hash position key aliasing.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool pseudo_legal(Move m) {

        Color  us   = sideToMove;
        Square from = m.from_sq();
        Square to   = m.to_sq();
        Piece  pc   = moved_piece(m);

        // Use a slower but simpler function for uncommon cases
        // yet we skip the legality check of MoveList<LEGAL>().
        if (m.type_of() != MoveType.NORMAL)
            return checkers() ? MoveList<GenType.EVASIONS>(*this).contains(m)
                              : MoveList<GenType.NON_EVASIONS>(*this).contains(m);

        // Is not a promotion, so the promotion piece must be empty
        Debug.Assert(m.promotion_type() - PieceType.KNIGHT == PieceType.NO_PIECE_TYPE);

        // If the 'from' square is not occupied by a piece belonging to the side to
        // move, the move is obviously not legal.
        if (pc == Piece.NO_PIECE || Color.color_of(pc) != us)
            return false;

        // The destination square cannot be occupied by a friendly piece
        if (pieces(us) & to)
            return false;

        // Handle the special case of a pawn move
        if (PieceType.type_of(pc) == PieceType.PAWN)
        {
            // We have already handled promotion moves, so destination cannot be on the 8th/1st rank
            if ((Bitboard.Rank8BB | Bitboard.Rank1BB) & to)
                return false;

            if (0==(Bitboard.pawn_attacks_bb(us, from) & pieces(~us) & to)  // Not a capture
                && !((from + Direction.pawn_push(us) == to) && empty(to))  // Not a single push
                && !((from + 2 * Direction.pawn_push(us) == to)            // Not a double push
                     && (Rank.relative_rank(us, from) == Rank.RANK_2) && empty(to) && empty(to - Direction.pawn_push(us))))
                return false;
        }
        else if (0==(Bitboard.attacks_bb(PieceType.type_of(pc), from, pieces()) & to))
            return false;

        // Evasions generator already takes care to avoid some kind of illegal moves
        // and legal() relies on this. We therefore have to take care that the same
        // kind of moves are filtered out here.
        if (checkers())
        {
            if (PieceType.type_of(pc) != PieceType.KING)
            {
                // Double check? In this case, a king move is required
                if (Bitboard.more_than_one(checkers()))
                    return false;

                // Our move must be a blocking interposition or a capture of the checking piece
                if (0==(Bitboard.between_bb(square(us, PieceType.KING), Bitboard.lsb(checkers())) & to))
                    return false;
            }
            // In case of king moves under check we have to remove the king so as to catch
            // invalid moves like b1a1 when opposite queen is on c1.
            else if (attackers_to_exist(to, pieces() ^ from, ~us))
                return false;
        }

        return true;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool capture(Move m){
        Debug.Assert(m.is_ok());
        return (!empty(m.to_sq()) && m.type_of() != MoveType.CASTLING) || m.type_of() == MoveType.EN_PASSANT;
    }
        
    // Returns true if a move is generated from the capture stage, having also
    // queen promotions covered, i.e. consistency with the capture stage move
    // generation is needed to avoid the generation of duplicate moves.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool capture_stage(Move m) {
        Debug.Assert(m.is_ok());
        return capture(m) || m.promotion_type() == PieceType.QUEEN;
    }
        
    // Tests whether a pseudo-legal move gives a check
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool gives_check(Move m) {

        Debug.Assert(m.is_ok());
        Debug.Assert(Color.color_of(moved_piece(m)) == sideToMove);

        Square from = m.from_sq();
        Square to   = m.to_sq();

        // Is there a direct check?
        if (check_squares(PieceType.type_of(piece_on(from))) & to)
            return true;

        // Is there a discovered check?
        if (blockers_for_king(~sideToMove) & from)
            return 0==(Bitboard.line_bb(from, to) & pieces(~sideToMove, PieceType.KING)) || m.type_of() == MoveType.CASTLING;

        if (m.type_of()==MoveType.NORMAL)
        {
            return false;
        }
        if (m.type_of() == MoveType.PROMOTION)
        {
            return 0!=(Bitboard.attacks_bb(m.promotion_type(), to, pieces() ^ from) & pieces(~sideToMove, PieceType.KING));
        }
        // En passant capture with check? We have already handled the case of direct
        // checks and ordinary discovered check, so the only case we need to handle
        // is the unusual case of a discovered check through the captured pawn.
        if (m.type_of() == MoveType.EN_PASSANT)
        {
            Square   capsq = Square.make_square(Square.file_of(to), Square.rank_of(from));
            Bitboard b     = (pieces() ^ from ^ capsq) | to;

            return 0!=((Bitboard.attacks_bb(square(~sideToMove, PieceType.KING), b, PieceType.ROOK) & pieces(sideToMove, PieceType.QUEEN, PieceType.ROOK))
                   | (Bitboard.attacks_bb(square(~sideToMove, PieceType.KING), b, PieceType.BISHOP)
                      & pieces(sideToMove, PieceType.QUEEN, PieceType.BISHOP)));
        }
        
        //default :  //CASTLING
        // Castling is encoded as 'king captures the rook'
        Square rto = Square.relative_square(sideToMove, to > from ? Square.SQ_F1 : Square.SQ_D1);

        return 0!=(check_squares(PieceType.ROOK) & rto);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Piece captured_piece() { return st.capturedPiece; }
    
    // Doing and undoing moves
    
    // Unmakes a move. When it returns, the position should
    // be restored to exactly the same state as before the move was made.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void undo_move(Move m) {

        Debug.Assert(m.is_ok());

        sideToMove = ~sideToMove;

        Color  us   = sideToMove;
        Square from = m.from_sq();
        Square to   = m.to_sq();
        Piece  pc   = piece_on(to);

        Debug.Assert(empty(from) || m.type_of() == MoveType.CASTLING);
        Debug.Assert(PieceType.type_of(st.capturedPiece) != PieceType.KING);

        if (m.type_of() == MoveType.PROMOTION)
        {
            Debug.Assert(Rank.relative_rank(us, to) == Rank.RANK_8);
            Debug.Assert(PieceType.type_of(pc) == m.promotion_type());
            Debug.Assert(PieceType.type_of(pc) >= PieceType.KNIGHT && PieceType.type_of(pc) <= PieceType.QUEEN);

            remove_piece(to);
            pc = Piece.make_piece(us, PieceType.PAWN);
            put_piece(pc, to);
        }

        if (m.type_of() == MoveType.CASTLING)
        {
            Square rfrom, rto;
            do_castling<false>(us, from, to, rfrom, rto);
        }
        else
        {
            move_piece(to, from);  // Put the piece back at the source square

            if (st.capturedPiece)
            {
                Square capsq = to;

                if (m.type_of() == MoveType.EN_PASSANT)
                {
                    capsq -= Direction.pawn_push(us);

                    Debug.Assert(PieceType.type_of(pc) == PieceType.PAWN);
                    Debug.Assert(to == st.previous.epSquare);
                    Debug.Assert(Rank.relative_rank(us, to) == Rank.RANK_6);
                    Debug.Assert(piece_on(capsq) == Piece.NO_PIECE);
                    Debug.Assert(st.capturedPiece == Piece.make_piece(~us, PieceType.PAWN));
                }

                put_piece(st.capturedPiece, capsq);  // Restore the captured piece
            }
        }

        // Finally point our state pointer back to the previous state
        st = st.previous;
        --gamePly;

        Debug.Assert(pos_is_ok());
    }
    // void       do_move(Move m, StateInfo& newSt, const TranspositionTable* tt);
    // DirtyPiece do_move(Move m, StateInfo& newSt, bool givesCheck, const TranspositionTable* tt);
    // void       do_null_move(StateInfo& newSt, const TranspositionTable& tt);
    
    // Must be used to undo a "null move"
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void undo_null_move() {

        Debug.Assert(0==checkers());

        st         = st.previous;
        sideToMove = ~sideToMove;
    }
    
    // Performs some consistency checks for the position object
    // and raise an assert if something wrong is detected.
    // This is meant to be helpful when debugging.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool pos_is_ok() {

        bool Fast = true;  // Quick (default) or full check?

        if ((sideToMove != Color.WHITE && sideToMove != Color.BLACK) || piece_on(square(Color.WHITE, PieceType.KING)) != Piece.W_KING 
            || piece_on(square(Color.BLACK, PieceType.KING)) != Piece.B_KING
            || (ep_square() != Square.SQ_NONE && Rank.relative_rank(sideToMove, ep_square()) != Rank.RANK_6))
            Debug.Assert(false, "pos_is_ok: Default");

        if (Fast)
            return true;

        if (pieceCount[Piece.W_KING] != 1 || pieceCount[Piece.B_KING] != 1
            || attackers_to_exist(square(~sideToMove, PieceType.KING), pieces(), sideToMove))
            Debug.Assert(false, "pos_is_ok: Kings");

        if (0!=(pieces(PieceType.PAWN) & (Bitboard.Rank1BB | Bitboard.Rank8BB)) || pieceCount[Piece.W_PAWN] > 8 || pieceCount[Piece.B_PAWN] > 8)
            Debug.Assert(false, "pos_is_ok: Pawns");

        if (0!=(pieces(Color.WHITE) & pieces(Color.BLACK)) || (pieces(Color.WHITE) | pieces(Color.BLACK)) != pieces()
            || Bitboard.popcount(pieces(Color.WHITE)) > 16 || Bitboard.popcount(pieces(Color.BLACK)) > 16)
            Debug.Assert(false, "pos_is_ok: Bitboards");

        for (PieceType p1 = PieceType.PAWN; p1 <= PieceType.KING; ++p1)
            for (PieceType p2 = PieceType.PAWN; p2 <= PieceType.KING; ++p2)
                if (p1 != p2 && 0!=(pieces(p1) & pieces(p2)))
                    Debug.Assert(false, "pos_is_ok: Bitboards");


        foreach (Piece pc in Pieces)
            if (pieceCount[pc] != Bitboard.popcount(pieces(Color.color_of(pc), PieceType.type_of(pc)))
                || pieceCount[pc] != board.Count(x => x == pc))
                Debug.Assert(false, "pos_is_ok: Pieces");

        foreach (Color c in Colors)
            foreach (CastlingRights cr in Castlingrights[c])
            {
                if (!can_castle(cr))
                    continue;

                if (piece_on(castlingRookSquare[cr]) != Piece.make_piece(c, PieceType.ROOK)
                    || castlingRightsMask[castlingRookSquare[cr]] != cr
                    || (castlingRightsMask[square(c, PieceType.KING)] & cr) != cr)
                    Debug.Assert(false, "pos_is_ok: Castling");
            }

        return true;
    }
}