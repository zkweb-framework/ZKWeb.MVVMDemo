var webpack = require('webpack');
var path = require('path');
var webpackMerge = require('webpack-merge');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CompressionPlugin = require("compression-webpack-plugin");
var CopyWebpackPlugin = require("copy-webpack-plugin");
var { CheckerPlugin } = require('awesome-typescript-loader');
var { TsConfigPathsPlugin } = require('awesome-typescript-loader');
var NamedModulesPlugin = require('webpack/lib/NamedModulesPlugin');

var webpackConfig = {
    entry: {
        polyfills: './src/polyfills.ts',
        vendor: './src/vendor.ts',
        app: './src/main.ts'
    },
    output: {
        publicPath: '/',
        path: path.resolve(__dirname, './dist-dev'),
    },
    plugins: [
        new CheckerPlugin(),
        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor', 'polyfills'],
        }),
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: './src/index-dev.html',
            inject: true,
            chunksSortMode: 'dependency'
        }),
        // https://github.com/AngularClass/angular-seed/issues/10
        new webpack.ContextReplacementPlugin(/angular(\\|\/)core(\\|\/)@angular/,
            path.resolve(__dirname, '../src')
        ),
        new CopyWebpackPlugin([
            { from: path.resolve(__dirname, "./src/vendor/images/favicon.ico"), to: "favicon.ico" },
            { from: path.resolve(__dirname, "./src/vendor/styles/preloader/preloader.css"), to: "preloader.css" },
        ]),
        new NamedModulesPlugin()
    ],
    module: {
        rules: [
            {
                test: /\.ts$/,
                loaders: [
                    'awesome-typescript-loader',
                    'angular-router-loader',
                    'angular2-template-loader'
                ],
            },
            {
                test: /\.js$/,
                loaders: ['babel-loader'],
                exclude: [/node_modules/, /dist/]
            },
            {
                test: /\.css$/,
                loaders: ['style-loader', 'css-loader']
            },
            {
                test: /\.scss$/,
                use: ['to-string-loader', 'css-loader', 'sass-loader']
            },
            {
                test: /\.html$/,
                loader: 'raw-loader'
            },
            {
                test: /\.woff(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=application/font-woff'
            },
            {
                test: /\.woff2(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=application/font-woff'
            },
            {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=application/octet-stream'
            },
            {
                test: /\.eot(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'file-loader'
            },
            {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=image/svg+xml'
            },
            {
                test: /\.(jpg|jpeg|bmp|png|gif)$/,
                loader: "file-loader"
            },
        ]
    }
};

var defaultConfig = {
    devtool: 'source-map',
    output: {
        filename: '[name].bundle.js',
        chunkFilename: '[id].chunk.js'
    },
    resolve: {
        extensions: ['.ts', '.js'],
        modules: [path.resolve(__dirname, 'node_modules')]
    },
    devServer: {
        contentBase: './',
        port: 3000,
        inline: true,
        stats: 'normal',
        historyApiFallback: true,
        compress: true
    },
    node: {
        global: true,
        crypto: 'empty',
        __dirname: true,
        __filename: true,
        Buffer: false,
        clearImmediate: false,
        setImmediate: false
    },
    watchOptions: {
        aggregateTimeout: 1000,
        poll: 1000,
        ignored: /(node_modules|dist)/
    },
};

module.exports = webpackMerge(defaultConfig, webpackConfig);
