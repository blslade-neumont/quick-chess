using System;
using System.Collections.Generic;
using System.Linq;
using ChessLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessLibraryTest
{
    [TestClass]
    public class TestBoard
    {
        [TestMethod]
        public void Init_Normal_QueensInSameColumn()
        {
            var board = new Board();
            board.Init("Normal");

            Assert.IsTrue(board[1, 4].piece.Type == Piece.PieceType.Queen);
            Assert.IsTrue(board[8, 4].piece.Type == Piece.PieceType.Queen);
            Assert.IsTrue(board[1, 5].piece.Type == Piece.PieceType.King);
            Assert.IsTrue(board[8, 5].piece.Type == Piece.PieceType.King);
        }

        [TestMethod]
        public void Init_Normal_HasAllPieces()
        {
            var board = new Board();
            board.Init("Normal");

            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.King, board.WhiteSide), 1);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Queen, board.WhiteSide), 1);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Bishop, board.WhiteSide), 2);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Knight, board.WhiteSide), 2);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Rook, board.WhiteSide), 2);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Pawn, board.WhiteSide), 8);

            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.King, board.BlackSide), 1);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Queen, board.BlackSide), 1);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Bishop, board.BlackSide), 2);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Knight, board.BlackSide), 2);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Rook, board.BlackSide), 2);
            Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Pawn, board.BlackSide), 8);
        }

        [TestMethod]
        public void Init_Normal_CenterIsEmpty()
        {
            var board = new Board();
            board.Init("Normal");

            for (var q = 3; q <= 6; q++)
            {
                for (var w = 1; w <= 8; w++)
                {
                    var piece = board[q, w].piece;
                    Assert.IsTrue(piece == null || piece.Type == Piece.PieceType.Empty);
                }
            }
        }

        [TestMethod]
        public void Init_Chess960_KingsBetweenRooks()
        {
            //Test multiple times to ensure it's not a fluke
            for (var q = 0; q < 20; q++)
            {
                var board = new Board();
                board.Init("Chess960");

                var blackRooks = FindPieceLocations(board, Piece.PieceType.Rook, board.BlackSide)
                    .OrderBy(cell => cell.row)
                    .ToArray();
                var blackKings = FindPieceLocations(board, Piece.PieceType.King, board.BlackSide).ToArray();
                Assert.AreEqual(blackRooks.Length, 2);
                Assert.AreEqual(blackKings.Length, 1);
                Assert.IsTrue(blackKings[0].col > blackRooks[0].col);
                Assert.IsTrue(blackKings[0].col < blackRooks[1].col);

                var whiteRooks = FindPieceLocations(board, Piece.PieceType.Rook, board.WhiteSide)
                    .OrderBy(cell => cell.row)
                    .ToArray();
                var whiteKings = FindPieceLocations(board, Piece.PieceType.King, board.WhiteSide).ToArray();
                Assert.AreEqual(whiteRooks.Length, 2);
                Assert.AreEqual(whiteKings.Length, 1);
                Assert.IsTrue(whiteKings[0].col > whiteRooks[0].col);
                Assert.IsTrue(whiteKings[0].col < whiteRooks[1].col);
            }
        }

        [TestMethod]
        public void Init_Chess960_AlternatingBishops()
        {
            //Test multiple times to ensure it's not a fluke
            for (var q = 0; q < 20; q++)
            {
                var board = new Board();
                board.Init("Chess960");

                var blackBishops = FindPieceLocations(board, Piece.PieceType.Bishop, board.BlackSide).ToArray();
                Assert.AreEqual(blackBishops.Length, 2);
                Assert.IsTrue(blackBishops[0].col % 2 != blackBishops[1].col % 2);

                var whiteBishops = FindPieceLocations(board, Piece.PieceType.Bishop, board.WhiteSide).ToArray();
                Assert.AreEqual(whiteBishops.Length, 2);
                Assert.IsTrue(whiteBishops[0].col % 2 != whiteBishops[1].col % 2);
            }
        }

        [TestMethod]
        public void Init_Chess960_HasAllPieces()
        {
            //Test multiple times to ensure it's not a fluke
            for (var q = 0; q < 20; q++)
            {
                var board = new Board();
                board.Init("Chess960");

                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.King, board.WhiteSide), 1);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Queen, board.WhiteSide), 1);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Bishop, board.WhiteSide), 2);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Knight, board.WhiteSide), 2);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Rook, board.WhiteSide), 2);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Pawn, board.WhiteSide), 8);

                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.King, board.BlackSide), 1);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Queen, board.BlackSide), 1);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Bishop, board.BlackSide), 2);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Knight, board.BlackSide), 2);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Rook, board.BlackSide), 2);
                Assert.AreEqual(CountAllPieces(board, Piece.PieceType.Pawn, board.BlackSide), 8);
            }
        }

        [TestMethod]
        public void Init_Chess960_CenterIsEmpty()
        {
            var board = new Board();
            board.Init("Chess960");

            for (var q = 3; q <= 6; q++)
            {
                for (var w = 1; w <= 8; w++)
                {
                    var piece = board[q, w].piece;
                    Assert.IsTrue(piece == null || piece.Type == Piece.PieceType.Empty);
                }
            }
        }

        #region Utility
        private int CountAllPieces(Board board, Piece.PieceType type, Side side)
        {
            return FindPieceLocations(board, type, side).Count();
        }
        private IEnumerable<Cell> FindPieceLocations(Board board, Piece.PieceType type, Side side)
        {
            for (var q = 1; q <= 8; q++)
            {
                for (var w = 1; w <= 8; w++)
                {
                    var place = board[q, w];
                    var piece = place.piece;
                    if (piece != null && piece.Side == side && piece.Type == type) yield return place;
                }
            }
        }
        #endregion
    }
}
