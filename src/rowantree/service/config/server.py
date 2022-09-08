""" Service Config Definition """

import configparser
import os
from typing import Optional

from pydantic import BaseModel


class ServerConfig(BaseModel):
    """
    Service Configuration

    Attributes
    ----------
    log_dir: Optional[str]
        The log directory.
    access_key: Optional[str]
        The API access key.
    database_server: Optional[str]
        The database server hostname
    database_name: Optional[str]
        The database name
    database_username: Optional[str]
        The database service account username
    database_password: Optional[str]
        The database service account password
    """

    log_dir: Optional[str]

    secret_key: Optional[str]
    algorithm: Optional[str]
    issuer: Optional[str]

    database_server: Optional[str]
    database_name: Optional[str]
    database_username: Optional[str]
    database_password: Optional[str]

    def __init__(self, *args, config_file_path: str = "rowantree.config", **kwargs):
        super().__init__(*args, **kwargs)
        config = configparser.ConfigParser()
        config.read(config_file_path)

        # Directory Options
        self.log_dir = config.get("DIRECTORY", "logs_dir")

        # Server Options
        self.secret_key = config.get("SERVER", "secret_key")
        self.algorithm = config.get("SERVER", "algorithm")
        self.issuer = config.get("SERVER", "issuer")

        # Database Options
        self.database_server = config.get("DATABASE", "server")
        self.database_name = config.get("DATABASE", "database")
        self.database_username = config.get("DATABASE", "username")
        self.database_password = config.get("DATABASE", "password")

        if "LOGS_DIR" in os.environ:
            self.log_dir = os.environ["LOGS_DIR"]

        if "ACCESS_TOKEN_SECRET_KEY" in os.environ:
            self.secret_key = os.environ["SECRET_KEY"]

        if "ACCESS_TOKEN_ALGORITHM" in os.environ:
            self.algorithm = os.environ["ALGORITHM"]

        if "ACCESS_TOKEN_ISSUER" in os.environ:
            self.issuer = os.environ["ACCESS_TOKEN_ISSUER"]

        if "DATABASE_SERVER" in os.environ:
            self.database_server = os.environ["DATABASE_SERVER"]

        if "DATABASE_NAME" in os.environ:
            self.database_name = os.environ["DATABASE_NAME"]

        if "DATABASE_USERNAME" in os.environ:
            self.database_username = os.environ["DATABASE_USERNAME"]

        if "DATABASE_PASSWORD" in os.environ:
            self.database_password = os.environ["DATABASE_PASSWORD"]
