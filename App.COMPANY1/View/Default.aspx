<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="App.COMPANY1.View.Default" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Management System</title>


    <%--<script src="http://www.jq22.com/jquery/jquery-1.10.2.js"></script>--%>

    <script src="/Core/jquery/jquery-3.1.1.js"></script>

    <link href="/Core/login/css/index_style.css" rel="stylesheet" />
    <link href="/Core/login/css/reset.css" rel="stylesheet" />
    <link href="/Core/login/css/style.css" rel="stylesheet" />
    <link href="/Core/login/css/supersized.css" rel="stylesheet" />



    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
            <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
        <![endif]-->
</head>
<body>

    <!-----HEADER STAR----->


    <div class="header" id="user_login">
        <div class="nav">
            <div class="page-container">
                <h1>Management System</h1>
                <form action="" method="post">
                    <div>
                        <input type="text" name="username" class="username" placeholder="账号" autocomplete="off" />
                    </div>
                    <div>
                        <input type="password" name="password" class="password" placeholder="密码" oncontextmenu="return false" onpaste="return false" />
                    </div>
                    <button id="submit" type="button" >登录</button>
                </form>
                <div class="connect">
                    <p>If we can only encounter each other rather than stay with each other,then I wish we had never encountered.</p>
                    <p style="margin-top: 20px;">如果只是遇见，不能停留，不如不遇见。</p>
                </div>
            </div>
            <div class="alert" style="display: none">
                <h2>消息</h2>
                <div class="alert_con">
                    <p id="ts"></p>
                    <p style="line-height: 70px"><a class="btn">确定</a></p>
                </div>
            </div>

            <!-- Javascript -->
<%--            <script src="http://apps.bdimg.com/libs/jquery/1.6.4/jquery.min.js" type="text/javascript"></script>--%>


            <script src="/Core/login/js/supersized-init.js"></script>
            <script src="/Core/login/js/supersized.3.2.7.min.js"></script>



<script>

    $(function () {


        //alert("11111111111111111");
        

        $(".btn").click(function () {
            is_hide();
        })

        var u = $("input[name=username]");
        var p = $("input[name=password]");

        var btn_submit = document.getElementById("submit");

        function goLogin() {

            if (u.val() == '' || p.val() == '') {
                $("#ts").html("用户名或密码不能为空~");
                is_show();
                return false;
            }
            else {
                var reg = /^[0-9A-Za-z]+$/;
                if (!reg.exec(u.val())) {
                    $("#ts").html("用户名错误");
                    is_show();
                    return false;
                }
            }

            console.log("u", u.val());

            $.post("/View/Handler/Login/LoginHandler.ashx?action1=123", { action: "GO_LOGIN", user_name: u.val(), user_pwd: p.val() }, function (data) {


                console.log("data:", data);

                //alert(data);


            }, "text");

        }

    });



            window.onload = function () {

                $(".connect p").eq(0).animate({ "left": "0%" }, 600);
                $(".connect p").eq(1).animate({ "left": "0%" }, 400);

                console.log("准备执行go_login");

                $.post("/View/Handler/Login/LoginHandler.ashx", { action: "GO_LOGIN", action1: "456", user_name: "詹姆斯", user_pwd: "15768091384" }, function (data) {

                    console.log("data:", data);
                    console.log("执行go_login完成");
                }, "text");

                console.log("准备执行go_test");
                $.post("/View/Handler/Login/LoginHandler.ashx", { action: "GO_TEST_CMODELLIST"}, function (data) {

                    console.log("data:", data);
                    console.log("准备执行go_test完成");
                }, "text");

            }

            function is_hide() { $(".alert").animate({ "top": "-500%" }, 300) }
            function is_show() { $(".alert").show().animate({ "top": "45%" }, 300) }

</script>

        </div>

        <div class="canvaszz"></div>
        <canvas id="canvas"></canvas>
    </div>


    <!-----HEADER END----->

    <!--用来解决视频右键菜单，用于视频上面的遮罩层 START-->
    <div class="videozz"></div>
    <!--用来解决视频右键菜单，用于视频上面的遮罩层 END-->

    <!--音乐 START-->
    <audio controls autoplay class="audio">
        <source src="/Core/login/css/Music.mp3" type="audio/mp3">
        <source src="/Core/login/css/Music.ogg" type="audio/ogg">
        <source src="/Core/login/css/Music.aac" type="audio/mp4">
    </audio>
    <!--音乐 END-->


    <script>
                //宇宙特效
                "use strict";
                var canvas = document.getElementById('canvas'),
                    ctx = canvas.getContext('2d'),
                    w = canvas.width = window.innerWidth,
                    h = canvas.height = window.innerHeight,

                    hue = 217,
                    stars = [],
                    count = 0,
                    maxStars = 1000;//星星数量

                var canvas2 = document.createElement('canvas'),
                    ctx2 = canvas2.getContext('2d');
                canvas2.width = 100;
                canvas2.height = 100;
                var half = canvas2.width / 2,
                    gradient2 = ctx2.createRadialGradient(half, half, 0, half, half, half);
                gradient2.addColorStop(0.025, '#CCC');
                gradient2.addColorStop(0.1, 'hsl(' + hue + ', 61%, 33%)');
                gradient2.addColorStop(0.25, 'hsl(' + hue + ', 64%, 6%)');
                gradient2.addColorStop(1, 'transparent');

                ctx2.fillStyle = gradient2;
                ctx2.beginPath();
                ctx2.arc(half, half, half, 0, Math.PI * 2);
                ctx2.fill();

                // End cache

                function random(min, max) {
                    if (arguments.length < 2) {
                        max = min;
                        min = 0;
                    }

                    if (min > max) {
                        var hold = max;
                        max = min;
                        min = hold;
                    }

                    return Math.floor(Math.random() * (max - min + 1)) + min;
                }

                function maxOrbit(x, y) {
                    var max = Math.max(x, y),
                        diameter = Math.round(Math.sqrt(max * max + max * max));
                    return diameter / 2;
                    //星星移动范围，值越大范围越小，
                }

                var Star = function () {

                    this.orbitRadius = random(maxOrbit(w, h));
                    this.radius = random(60, this.orbitRadius) / 8;
                    //星星大小
                    this.orbitX = w / 2;
                    this.orbitY = h / 2;
                    this.timePassed = random(0, maxStars);
                    this.speed = random(this.orbitRadius) / 50000;
                    //星星移动速度
                    this.alpha = random(2, 10) / 10;

                    count++;
                    stars[count] = this;
                }

                Star.prototype.draw = function () {
                    var x = Math.sin(this.timePassed) * this.orbitRadius + this.orbitX,
                        y = Math.cos(this.timePassed) * this.orbitRadius + this.orbitY,
                        twinkle = random(10);

                    if (twinkle === 1 && this.alpha > 0) {
                        this.alpha -= 0.05;
                    } else if (twinkle === 2 && this.alpha < 1) {
                        this.alpha += 0.05;
                    }

                    ctx.globalAlpha = this.alpha;
                    ctx.drawImage(canvas2, x - this.radius / 2, y - this.radius / 2, this.radius, this.radius);
                    this.timePassed += this.speed;
                }

                for (var i = 0; i < maxStars; i++) {
                    new Star();
                }

                function animation() {
                    ctx.globalCompositeOperation = 'source-over';
                    ctx.globalAlpha = 0.5; //尾巴
                    ctx.fillStyle = 'hsla(' + hue + ', 64%, 6%, 2)';
                    ctx.fillRect(0, 0, w, h)

                    ctx.globalCompositeOperation = 'lighter';
                    for (var i = 1, l = stars.length; i < l; i++) {
                        stars[i].draw();
                    };

                    window.requestAnimationFrame(animation);
                }

                animation();
    </script>

</body>
</html>


<%--不兼容Vue--%>
<%--<script src="/Core/vue/vue.js"></script>--%>


<script>


          var l_user = new Vue({
                    el: '#user_login',
                    data: {
                        login_name: '',
                        login_pwd: ''
                    },
                    methods: {
                        login_btn: function () {

                            alert("123");

                            var u = $("input[name=username]");
                            var p = $("input[name=password]");

                            if (u.val() == '' || p.val() == '') {
                                $("#ts").html("用户名或密码不能为空~");
                                is_show();
                                return false;
                            }
                            else {
                                var reg = /^[0-9A-Za-z]+$/;
                                if (!reg.exec(u.val())) {
                                    $("#ts").html("用户名错误");
                                    is_show();
                                    return false;
                                }
                            }



                        }
                    }
                });


</script>
