const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: {
        main: path.join(__dirname, 'ts/main.ts'),
        validation: path.join(__dirname, 'ts/validation.ts')
    },
    output: {
        path: path.join(__dirname, 'dist')
    },
    module: {
        rules: [
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
        extensions: ['.ts', '.tsx', '.js', '.jsx']
    },
    plugins: [new CopyPlugin(['favicon.ico', {from:'assets', to:'assets'}])],
    optimization: {
        splitChunks: {
            chunks: 'all',
            name: 'vendor'
        }
    }
};
