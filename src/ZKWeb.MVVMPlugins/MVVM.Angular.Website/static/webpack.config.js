var webpack = require('webpack');
var path = require('path');
var webpackMerge = require('webpack-merge');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CompressionPlugin = require("compression-webpack-plugin");
var ngtools = require('@ngtools/webpack');
var CopyWebpackPlugin = require("copy-webpack-plugin");

var webpackConfig = {
    entry: {
        polyfills: './src/polyfills.ts',
        vendor: './src/vendor.ts',
        app: './src/main.ts'
    },
    output: {
        publicPath: '/',
        path: path.resolve(__dirname, './dist'),
    },
    plugins: [
        new ngtools.AotPlugin({
            tsConfigPath: path.resolve(__dirname, './tsconfig.json'),
            skipMetadataEmit: false,
            entryModule: path.resolve(__dirname, './src/modules/app_module/app.module#AppModule'),
            compilerOptions: {
                emitDecoratorMetadata: true,
                experimentalDecorators: true,
                sourceMap: true,
            }
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor', 'polyfills'],
            // minChunks: 2,
            // children: true,
            // async: true
        }),
        new HtmlWebpackPlugin({
            filename: 'index.html',
            template: './src/index.html',
            inject: true,
            chunksSortMode: 'dependency'
        }),
        new CompressionPlugin({
            asset: "[path].gz[query]",
            algorithm: "gzip",
            test: /\.js$|\.html$/
        }),
        new webpack.optimize.UglifyJsPlugin({ minimize: false }),
        new CopyWebpackPlugin([
            { from: path.resolve(__dirname, "./src/vendor/images/favicon.ico"), to: "favicon.ico" },
            { from: path.resolve(__dirname, "./src/vendor/styles/preloader/preloader.css"), to: "preloader.css" },
        ]),
    ],
    module: {
        rules: [
            {
                test: /\.ts$/,
                loaders: ['@ngtools/webpack'],
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
    devtool: 'inline-source-map',
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
        stats: 'errors-only',
        historyApiFallback: true,
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
