var webpack = require("webpack");
var Precss = require("precss");
var Autoprefixer = require("autoprefixer");
var path = require('path');
var asTsLoader = require('awesome-typescript-loader');

const workingPath = "./React";

module.exports = {
    entry: {
        userSettings: workingPath + "/messageHubClient/userSettings/main.tsx",
        userMessages: workingPath + "/messageHubClient/userMessages/main.tsx",
        administerMessage: workingPath + "/messageHubClient/administer/main.tsx",
        handleOrganizationUser: workingPath + "/organizationClient/handleUser/main.tsx",
        addUserToOrganization: workingPath + "/organizationClient/addUser/main.tsx",
        customerCardUserList: workingPath + "/CustomerCardUserList/userList.tsx",
    },
    output: {
        path: path.resolve(__dirname, './Static/dist/scripts'),
        filename: '[name].production.js'
    },
    // Turn on sourcemaps
    //devtool: "source-map",
    devtool: 'cheap-module-source-map',
    //devtool: 'eval',
    externals: {
        "react-router": "ReactRouter",
        "react": "React",
        "react-dom": "ReactDOM"
    },
    resolve: {
        extensions: [".ts", ".js", ".tsx", ".scss"],
        modules: ["components", "node_modules", "store", "styles"]
    },
    // plugins: [
    //     new webpack.HotModuleReplacementPlugin(),
    //     new ExtractTextPlugin("[name].css")
    // ],
    module: {
        rules: [
            {
                test: /\.scss$/,
                loader: "style-loader!css-loader!postcss-loader!sass-loader?outputStyle=expanded",
                exclude: /node_modules/
                //loader: ExtractTextPlugin.extract("css!postcss!sass?outputStyle=expanded")
            },
            {
                test: /\.(woff|ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
                exclude: /node_modules/,
                loader: "base64-font-loader"
            },
            {
                test: /\.ts(x?)$/,
                exclude: /(node_modules|lib)/,
                loaders: ["awesome-typescript-loader"]
            }
        ]
    },
//    postcss: function() {
//        return [Precss, Autoprefixer];
//    },
//    node: {
//        fs: "empty",
//        tls: "empty",
//        net: "empty"
//    },
    plugins: [
        new webpack.ProvidePlugin({
            Promise: "imports-loader?this=>global!exports-loader?global.Promise!es6-promise"
            //fetch: 'imports?this=>global!exports?global.fetch!fetch'
        }),
        // new ExtractTextPlugin("[name].styles.css", {
        //     allChunks: true
        // }),
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false
            },
            output: {
                comments: false
            }
        }),
        new webpack.DefinePlugin({
            'process.env': {
                'NODE_ENV': JSON.stringify('production')
            }
        })
    ]
}
