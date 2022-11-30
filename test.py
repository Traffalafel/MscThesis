import sys
import math

n = int(sys.argv[1])
K = float(sys.argv[2])

E = 0
num_is = K*(n-2)/n
print(num_is)
num_is = int(num_is)
print(num_is)

def prob(n, k, i, m):
    return 2 * (1/n + i/k) * (1 - 1/n - i/k) * math.pow(1 - 1/n, m-1)

for m in range(1, n+1):
    E += sum(1/prob(n, K, i, m) for i in range(0, num_is))

print(E)
