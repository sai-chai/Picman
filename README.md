# Picman
### Rudimentary 3D maze navigation game, complete with ghosts to run from, 'dots' to eat, and coins that allow you to have a go at your enemies.
Rights disclaimer: this game is a derivative work of Pac-Man®, rights reserved by Bandai Namco Ent. It was created for educational purposes under the conditions of Fair Use, pursuant to 17 U.S.C. § 107, related admendments, and case law.
## Demo
[![Picman Demo](http://img.youtube.com/vi/YcfnMLzye-E/0.jpg)](https://www.youtube.com/watch?v=YcfnMLzye-E "Picman Demo")

## Maze Generation
Uses a DFS algorithm to randomly generate the edges of the maze. A sequence of numbers from 0 to n-1, where n is the total number of cells, is shuffled randomly. Once all cells are generated and edges are determined, each cell is assigned a spawn number from the shuffled sequence, a modulus of which is used by the MazeCell instance to determine whether the cell will spawn a coin or dot. Difficulty rises exponentially with maze size, not just because of size alone, but also the scarcity of coins.

## Ghost AI
Four ghosts are spawned by the Game singleton at the four centermost cells of the Maze. Each one uses a NavMeshAgent to pursue the player-character. If a coin is obtained, scatter mode is activated by the FirstPerson instance, and the ghosts flee the player-character for 12.5 seconds by setting their destinations to their original starting cells in the center of the maze. 

## Assets
* Dots
* Coins
* Ghosts
* MazeCells
* Maze
* FirstPerson/Pacman (player-character)
* Game (singleton)
