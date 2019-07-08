const fs = require('fs');
const path = require('path');
const glob = require('glob');

const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyPlugin = require('copy-webpack-plugin');
const { TsConfigPathsPlugin } = require('awesome-typescript-loader');

const entries = (() => {
    const pagesPath = path.join(__dirname, 'ts/pages');
    console.log(`pagesPath: ${pagesPath}`);
    var pages = glob.sync(`${pagesPath}/**/*.ts`);
    return pages.reduce((acc, item) => {
        var relPath = path.relative(pagesPath, item);
        var keyName = relPath.replace(/\.[^/.]+$/, "");
        acc[keyName] = item;
        return acc;
    }, {
        'main': path.join(__dirname, 'ts/main.ts')
    });
})();

for (let key in entries) {
    console.log(key);
}

module.exports = {
    entry: entries,
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
