const path = require('path');
const fs = require('fs');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

const entries = {};
function walkDir(dir) {
    fs.readdirSync(dir).forEach(file => {
        const filePath = path.resolve(dir, file);
        const isDirectory = fs.statSync(filePath).isDirectory();
        if (isDirectory) {
            walkDir(filePath);
        } else if (path.extname(filePath) === '.ts') {
            const entryName = path.basename(file, '.ts');
            entries[entryName] = filePath;
        }
    });
}

walkDir(path.resolve(__dirname, 'src', 'ts'));

module.exports = {
    entry: entries,
    output: {
        filename: '[name].entry.js',
        path: path.resolve(__dirname, '..', 'wwwroot', 'dist'),
        clean: true
    },
    devtool: 'source-map',
    mode: 'development',
    resolve: {
        extensions: [".ts", ".js"],
        extensionAlias: {'.js': ['.js', '.ts']}
    },
    module: {
        rules: [
            {
                test: /\.s?css$/,
                use: [{loader: MiniCssExtractPlugin.loader}, 'css-loader', 'sass-loader']
            },
            {
                test: /\.ts$/i,
                use: ['ts-loader'],
                exclude: /node_modules/
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css"
        })
    ]
};