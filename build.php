<?php

/*
 * iTXTech SharpFlashDetector
 *
 * Copyright (C) 2020 iTX Technologies
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

function between(string $string, string $after, string $before, int $offset = 0) : string{
	return substr($string, $pos = strpos($string, $after, $offset) + strlen($after),
		strpos($string, $before, $pos) - $pos);
}

$pipes = [];
$proc = proc_open("git rev-parse --short HEAD", [
	0 => ["pipe", "r"],
	1 => ["pipe", "w"],
	2 => ["pipe", "w"]
], $pipes, $argv[1]);

if(is_resource($proc)){
	$rev = stream_get_contents($pipes[1]);
	proc_close($proc);
}

$manifest = file_get_contents($argv[2]);
$version = between($manifest, "<Version>", "</Version>");
$ver = explode("-", $version)[0] . "-" . trim($rev);
file_put_contents($argv[2], str_replace($version, $ver, $manifest));

echo "New version generated: $ver" . PHP_EOL;
