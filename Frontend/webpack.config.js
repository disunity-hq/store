const { TsConfigPathsPlugin } = require('awesome-typescript-loader');

const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: path.join(__dirname, 'ts/main.ts'),
    output: {
        path: path.join(__dirname, 'dist'),
        libraryTarget: 'window',
    },
    module: {
        rules: [
            {
                test: /\.html$/,
                exclude: /node_modules/,
                loader: "html-loader?exportAsEs6Default"
            },
            {
                test: /\.tsx?$/,
                loader: 'awesome-typescript-loader',
                exclude: /node_modules/
            },
            {
                test: /\.(css|scss|sass)$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    'css-loader',
                    {
                        loader: 'postcss-loader',
                        options: {
                            plugins: function () {
                                return [require('autoprefixer')];
                            }
                        }
                    },
                    'sass-loader'
                ]
            }
        ]
    },
    resolve: {
        plugins: [ new TsConfigPathsPlugin() ],
        extensions: ['.ts', '.tsx', '.js', '.jsx', '.html']
    },
    plugins: [new CopyPlugin(['favicon.ico', {from:'assets', to:'assets'}])],
};
