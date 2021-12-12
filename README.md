# highload-sw-arch-hw-12

# Prerequisites
* docker
* linux + bash

# 1. Run Redis instances with rdb and aof + beanstalkd and run benchmarks
```
./run.sh
```

| Technology | Beanstalkd | Redis AOF | Redis RDB |
| :---:   | :-: | :-: | :-: |
| Produce throughput req/sec | 194 | 100 | 99 |
| Consume throughput req/sec | 184 | 61 | 97 |

# 2. Clean up
```
./cleanup.sh
```