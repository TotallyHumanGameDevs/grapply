# Grapply

![grrapply](/images/grapply_anim.gif)

#### Grapply is a simple game where you use a grappling hook to fly around the screen to reach goals as fast as possible.

I wrote this game as a test bed to experiment with and write code that efficiently generates catenary curves between two points in real time (to simulate a rope going slack) and to do gravity and centrifugal force simulation.

I found that using an iterative process (Newton’s method) to solve for the catenary curve gave me real-time results and accuracy. I’ve made this code open source in the hopes that someone else might find the particular piece of code that generates catenary curves useful. You'll find that specific code here:

[/Assets/Rope.cs](/Assets/Rope.cs)

 
#### Good reads on catenary curves and other links that were helpful for writing the code:

https://en.wikipedia.org/wiki/Catenary
https://proofwiki.org/wiki/Equation_of_Catenary
https://math.stackexchange.com/questions/1000447/finding-the-catenary-curve-with-given-arclength-through-two-given-points
https://en.wikipedia.org/wiki/Newton%27s_method

![Catenary arch](https://upload.wikimedia.org/wikipedia/commons/thumb/f/f7/Catenary-pm.svg/350px-Catenary-pm.svg.png)

#### Interesting fact
![Catenary arch](https://images.ukdissertations.com/110/0503291.002.jpg)

Does this look like an inverted catenary curve? It does! That's because catenaries are used in architecture and engineering in the design of bridges and arches so that forces do not result in bending moments. (from https://en.wikipedia.org/wiki/Catenary_arch)
 
