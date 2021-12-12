#! /bin/bash

docker-compose up -d

echo "Redis and beanstalkd are up and running."

# run benchmarks

siege -d1 -c50 -t15s http://localhost:8080/beanstalkd/produce
siege -d1 -c50 -t15s http://localhost:8080/beanstalkd/consume

siege -d1 -c50 -t15s http://localhost:8080/redis-rdb/produce
siege -d1 -c50 -t15s http://localhost:8080/redis-rdb/consume

siege -d1 -c50 -t15s http://localhost:8080/redis-aof/produce
siege -d1 -c50 -t15s http://localhost:8080/redis-aof/consume