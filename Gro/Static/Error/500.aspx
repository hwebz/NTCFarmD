<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="Gro.Error._500" %>

<% Response.StatusCode = 500; %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Server error</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.6.3/css/font-awesome.min.css" />

    <link rel="stylesheet" href="~/Static/dist/styles/styles.min.css" />
</head>
<body>
    <section class="lm__contents iam-block background-type-2">
        <div class="wrapper">
            <div class="layout  layout--large">
                <div class="layout__item u-1/1">
                    <div class="lm__center-block type-3">
                        <div class="iam-block__logo">
                            <h1 class="primary clarendonurw-reg-font align-center-text">LM<span class="super-text">2</span></h1>
                            <h2 class="slogan clarendonurw-reg-font align-center-text">Verktyget för jordens entreprenörer</h2>
                        </div>
                        <div class="iam-block__box__wrapper">
                            <div class="iam-block__box">
                                <div class="forgot-password__description">
                                    <h1 class="primary black-text clarendonurw-reg-font align-center-text">500</h1>
                                    <h2 class="slogan black-text clarendonurw-reg-font align-center-text">Något fungerar inte riktigt som det ska</h2>
                                    <p>Försök igen lite senare eller gå till LM<sup>2</sup>  startsida</p>
                                </div>
                            </div>
                            <div class="iam-block__links">
                                <a class="lm__bold-title not-responsive" href="/">Gå vidare till Startsidan <i class="fa fa-long-arrow-right" aria-hidden="true"></i></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>
</html>
