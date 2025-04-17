pipeline {
    agent any
    options {
        skipStagesAfterUnstable()
    }
    stages {
        stage('Build') {
            steps {
                sh 'dotnet restore'
                sh 'dotnet build --no-restore'
            }
        }
        stage('Test') {
            steps {
                sh 'dotnet test --no-build --no-restore --collect "XPlat Code Coverage"'
            }
            post {
                always {
                    recordCoverage(tools: [[parser: 'COBERTURA', pattern: '**/coverage.xml']], sourceDirectories: [[path: 'CurrencyRate.Api.Test/TestResults']])
                }
            }
        }
        stage('Image build and push') { 
            steps {
                script {
                    docker.withRegistry('', 'dh-credentials') {
                        def customImage = docker.build("maksimaleksandrovich/crrency-rate:latest", "./src/CurrencyRate.Api")
                        customImage.push()
                    }
                }
            }
        }
    }
}