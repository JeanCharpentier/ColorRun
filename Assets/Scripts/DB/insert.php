<?php
include 'credentials.php';

$sqlco = new mysqli($host,$user,$pwd,$db);

$player = strip_tags($_POST['player']); // Revoir pour la sécu et les injections SQL
$score = strip_tags($_POST['score']);

if($player != "" && $score != "") {
    if($sqlco->connect_error) {
        die("Connection failed: ".$sqlco->connect_error);
    }

    $query = "SELECT Player FROM lb_normal WHERE Player = '".$player."';";

    $res = $sqlco->query($query);

    if($res->num_rows > 0)
    {
        // UPDATE
        $newQuery = "UPDATE lb_normal SET HighScore = ".$score." WHERE lb_normal.Player = '".$player."';";
    }else {
        // INSERT
        $newQuery = "INSERT INTO lb_normal (Id, Player, HighScore) VALUES (NULL, '".$player."', '".$score."');";
    }
    echo $newQuery;
    $newRes = $sqlco->query($newQuery);

    $sqlco->close();
}
?>