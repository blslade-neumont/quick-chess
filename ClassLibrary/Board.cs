/***************************************************************
 * File: Board.cs
 * Created By: Syed Ghulam Akbar        Date: 27 June, 2005
 * Description: The main chess board. Board contain the chess cell
 * which will contain the chess pieces. Board also contains the methods
 * to get and set the user moves.
 ***************************************************************/

using System;
using System.Collections;
using System.Xml;

namespace ChessLibrary
{
    /// <summary>
    /// he main chess board. Board contain the chess cell
    /// which will contain the chess pieces. Board also contains the methods
    /// to get and set the user moves.
    /// </summary>
    [Serializable]
    public class Board
    {
        private Side m_WhiteSide, m_BlackSide;    // Chess board site object 
        private Cells m_cells;    // collection of cells in the board

        public Board()
        {
            m_WhiteSide = new Side(Side.SideType.White);    // Makde white side
            m_BlackSide = new Side(Side.SideType.Black);    // Makde white side

            m_cells = new Cells();                    // Initialize the chess cells collection
        }

        // Initialize the chess board and place piece on thier initial positions
        public void Init(string gameMode)
        {
            m_cells.Clear();        // Remove any existing chess cells

            // Build the 64 chess board cells
            for (int row=1; row<=8; row++)
                for (int col=1; col<=8; col++)
                {
                    m_cells.Add(new Cell(row,col));    // Initialize and add the new chess cell
                }

            if (gameMode == "Chess960") this.InitChess960();
            else this.InitNormal();
        }

        private void InitNormal()
        {
            // Now setup the board for black side
            m_cells["a1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            m_cells["h1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            m_cells["b1"].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
            m_cells["g1"].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
            m_cells["c1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            m_cells["f1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            m_cells["e1"].piece = new Piece(Piece.PieceType.King, m_BlackSide);
            m_cells["d1"].piece = new Piece(Piece.PieceType.Queen, m_BlackSide);
            for (int col = 1; col <= 8; col++)
                m_cells[2, col].piece = new Piece(Piece.PieceType.Pawn, m_BlackSide);

            // Now setup the board for white side
            m_cells["a8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
            m_cells["h8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
            m_cells["b8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
            m_cells["g8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
            m_cells["c8"].piece = new Piece(Piece.PieceType.Bishop, m_WhiteSide);
            m_cells["f8"].piece = new Piece(Piece.PieceType.Bishop, m_WhiteSide);
            m_cells["e8"].piece = new Piece(Piece.PieceType.King, m_WhiteSide);
            m_cells["d8"].piece = new Piece(Piece.PieceType.Queen, m_WhiteSide);
            for (int col = 1; col <= 8; col++)
                m_cells[7, col].piece = new Piece(Piece.PieceType.Pawn, m_WhiteSide);
        }

        private void InitChess960()
        {
            var rnd = new Random();

            this.InitChess960_Side(rnd, m_BlackSide, 1, 2);
            this.InitChess960_Side(rnd, m_WhiteSide, 8, 7);
        }
        private void InitChess960_Side(Random rnd, Side side, int homeRow, int pawnRow)
        {
            var king = rnd.Next(2, 8);
            var rook1 = rnd.Next(1, king);
            var rook2 = rnd.Next(king + 1, 9);
            int bishop1 = king, bishop2 = king;
            while (bishop1 == king || bishop1 == rook1 || bishop1 == rook2)
                bishop1 = rnd.Next(1, 9);
            while (bishop2 == king || bishop2 == rook1 || bishop2 == rook2 || bishop1 % 2 == bishop2 % 2)
                bishop2 = rnd.Next(1, 9);

            //TODO MAYBE: handle these three more efficiently?
            int knight1 = king, knight2 = king, queen = king;
            while (knight1 == king || knight1 == rook1 || knight1 == rook2 || knight1 == bishop1 || knight1 == bishop2)
                knight1 = rnd.Next(1, 9);
            while (knight2 == king || knight2 == rook1 || knight2 == rook2 || knight2 == bishop1 || knight2 == bishop2 || knight2 == knight1)
                knight2 = rnd.Next(1, 9);
            while (queen == king || queen == rook1 || queen == rook2 || queen == bishop1 || queen == bishop2 || queen == knight1 || queen == knight2)
                queen = rnd.Next(1, 9);

            m_cells[homeRow, rook1].piece = new Piece(Piece.PieceType.Rook, side);
            m_cells[homeRow, rook2].piece = new Piece(Piece.PieceType.Rook, side);
            m_cells[homeRow, knight1].piece = new Piece(Piece.PieceType.Knight, side);
            m_cells[homeRow, knight2].piece = new Piece(Piece.PieceType.Knight, side);
            m_cells[homeRow, bishop1].piece = new Piece(Piece.PieceType.Bishop, side);
            m_cells[homeRow, bishop2].piece = new Piece(Piece.PieceType.Bishop, side);
            m_cells[homeRow, king].piece = new Piece(Piece.PieceType.King, side);
            m_cells[homeRow, queen].piece = new Piece(Piece.PieceType.Queen, side);
            for (int col = 1; col <= 8; col++)
                m_cells[pawnRow, col].piece = new Piece(Piece.PieceType.Pawn, side);
        }

        // get the new item by rew and column
        public Cell this[int row, int col]
        {
            get
            {
                return m_cells[row, col];
            }
        }

        // get the new item by string location
        public Cell this[string strloc]
        {
            get
            {
                return m_cells[strloc];    
            }
        }

        // get the chess cell by given cell
        public Cell this[Cell cellobj]
        {
            get
            {
                return m_cells[cellobj.ToString()];    
            }
        }

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlBoard = xmlDoc.CreateElement("Board");

            // Append game state attributes
            xmlBoard.AppendChild(m_WhiteSide.XmlSerialize(xmlDoc));
            xmlBoard.AppendChild(m_BlackSide.XmlSerialize(xmlDoc));

            xmlBoard.AppendChild(m_cells.XmlSerialize(xmlDoc));

            // Return this as String
            return xmlBoard;
        }

        /// <summary>
        /// DeSerialize the Board object from XML String
        /// </summary>
        /// <returns>XML containing the Board object state XML</returns>
        public void XmlDeserialize(XmlNode xmlBoard)
        {
            // Deserialize the Sides XML
            XmlNode side = XMLHelper.GetFirstNodeByName(xmlBoard, "Side");

            // Deserialize the XML nodes
            m_WhiteSide.XmlDeserialize(side);
            m_BlackSide.XmlDeserialize(side.NextSibling);

            // Deserialize the Cells
            XmlNode xmlCells = XMLHelper.GetFirstNodeByName(xmlBoard, "Cells");
            m_cells.XmlDeserialize(xmlCells);
        }

        // get all the cell locations on the chess board
        public ArrayList GetAllCells()
        {
            ArrayList CellNames = new ArrayList();

            // Loop all the squars and store them in Array List
            for (int row=1; row<=8; row++)
                for (int col=1; col<=8; col++)
                {
                    CellNames.Add(this[row,col].ToString()); // append the cell name to list
                }

            return CellNames;
        }

        // get all the cell containg pieces of given side
        public ArrayList GetSideCell(Side.SideType PlayerSide)
        {
            ArrayList CellNames = new ArrayList();

            // Loop all the squars and store them in Array List
            for (int row=1; row<=8; row++)
                for (int col=1; col<=8; col++)
                {
                    // check and add the current type cell
                    if (this[row,col].piece!=null && !this[row,col].IsEmpty() && this[row,col].piece.Side.type == PlayerSide)
                        CellNames.Add(this[row,col].ToString()); // append the cell name to list
                }

            return CellNames;
        }

        // Returns the cell on the top of the given cell
        public Cell TopCell(Cell cell)
        {
            return this[cell.row-1, cell.col];
        }

        // Returns the cell on the left of the given cell
        public Cell LeftCell(Cell cell)
        {
            return this[cell.row, cell.col-1];
        }

        // Returns the cell on the right of the given cell
        public Cell RightCell(Cell cell)
        {
            return this[cell.row, cell.col+1];
        }

        // Returns the cell on the bottom of the given cell
        public Cell BottomCell(Cell cell)
        {
            return this[cell.row+1, cell.col];
        }

        // Returns the cell on the top-left of the current cell
        public Cell TopLeftCell(Cell cell)
        {
            return this[cell.row-1, cell.col-1];
        }

        // Returns the cell on the top-right of the current cell
        public Cell TopRightCell(Cell cell)
        {
            return this[cell.row-1, cell.col+1];
        }

        // Returns the cell on the bottom-left of the current cell
        public Cell BottomLeftCell(Cell cell)
        {
            return this[cell.row+1, cell.col-1];
        }

        // Returns the cell on the bottom-right of the current cell
        public Cell BottomRightCell(Cell cell)
        {
            return this[cell.row+1, cell.col+1];
        }
    }
}
