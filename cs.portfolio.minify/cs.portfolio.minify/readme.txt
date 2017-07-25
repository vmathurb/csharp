Problem - Portfolio minify (Cheapify)

Find the cheapest set of trades that give us maximum testing coverage in terms of
 - Valuation models
 - Market dependencies
 - Static dependencies

Facts
 - Each trade has a cost of computation (ElapsedTime or BatchCost)
 - Each trade has a cost in terms of result size (Longer dated trades have more cashflows, worth considering?)
 - Each trade has a set of dependencies (Market, Static)

Constraints
 - We must have at least one trade each per trade type ensuring valuation model coverage

Modeling as Weighted Set Cover problem

Given a set of elements E = {e1,e2,...,en} and a set of m subsets of E,S = {S1,S2,...,Sn}, 
find a “least cost” collection C of sets from S such that C covers all elements in E. 

Set Cover comes in two flavors, unweighted and weighted. In unweighted Set Cover, the cost of a collection C is 
number of sets contained in it. In weighted Set Cover, there is a nonnegative weight function w:S->R, and the 
cost of C is defined to be its total weight, i.e., sum of w(Si) where Si is all sets belonging to C.

Portfolio - S
Trades - S1,S2,.....,Sn
Total portfolio dependencies - E
Dependencies - e1,e2,.......,en
Trade cost function - w : S -> R
Cheapified Portfolio - C

Python Algorithm

U = set([1,2,3,4])
R = U
S = [set([1,2]), 
     set([1]), 
     set([1,2,3]), 
     set([1]), 
     set([3,4]), 
     set([4]), 
     set([1,2]), 
     set([3,4]), 
     set([1,2,3,4])]
w = [1, 1, 2, 2, 2, 3, 3, 4, 4]

C = []
costs = []

def findMin(S, R):
    minCost = 99999.0
    minElement = -1
    for i, s in enumerate(S):
        try:
            cost = w[i]/(len(s.intersection(R)))
            if cost < minCost:
                minCost = cost
                minElement = i
        except:
            # Division by zero, ignore
            pass
    return S[minElement], w[minElement]

while len(R) != 0:
    S_i, cost = findMin(S, R)
    C.append(S_i)
    R = R.difference(S_i)
    costs.append(cost)

print "Cover: ", C
print "Total Cost: ", sum(costs), costs

