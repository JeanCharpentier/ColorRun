<?php // Merci AnataGawa !!!
    require_once('./credentials.php');
    
    try {
        $dbh = new PDO('mysql:host='. $host .';dbname='. $db, $user, $pwd);
    } catch(PDOException $e) {
        echo '<h1>An error has occurred.</h1><pre>', $e->getMessage() ,'</pre>';
    }

    $sth = $dbh->query('SELECT Player, HighScore FROM lb_normal ORDER BY HighScore DESC LIMIT 12;' );
    $sth->setFetchMode(PDO::FETCH_ASSOC);

    $result = $sth->fetchAll();
    
    header('Content-Type: application/json');
    echo json_encode($result);
?>