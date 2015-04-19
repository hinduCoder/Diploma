package com.vedroid.diplomaassistant

import android.app.Activity
import android.content.ContentValues
import android.content.Context
import android.content.Intent
import android.content.SharedPreferences
import android.graphics.Bitmap
import android.graphics.BitmapFactory
import android.net.LocalSocket
import android.net.LocalSocketAddress
import android.net.Uri
import android.os.Bundle
import android.os.Environment
import android.provider.MediaStore
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.Toast
import butterknife.bindView
import org.apache.http.impl.io.SocketOutputBuffer
import java.io.ByteArrayOutputStream
import java.io.File
import java.io.IOException
import java.net.InetAddress
import java.net.InetSocketAddress
import java.net.Socket
import java.nio.channels.ByteChannel
import java.nio.channels.SocketChannel
import java.nio.channels.spi.SelectorProvider
import java.text.SimpleDateFormat
import java.util.Calendar
import java.util.Date


public class MainActivity : Activity() {

    private val ipField:EditText by bindView(R.id.id_field)
    private val sendButton: Button by bindView(R.id.send_button)

    var pathToTempFile: String? = null
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        ipField.setText(getPreferences(Context.MODE_PRIVATE).getString("ip", ""))
        sendButton.setOnClickListener {onSendButtonClick()}
    }

    private fun onSendButtonClick() {
        val editor = getPreferences(Context.MODE_PRIVATE).edit()
        editor.putString("ip", ipField.getText().toString())
        editor.apply()

        val intent = Intent(MediaStore.ACTION_IMAGE_CAPTURE)
        intent.putExtra(MediaStore.EXTRA_OUTPUT, getUriToSaveTemp());
        startActivityForResult(intent, 0)
    }

    private fun getUriToSaveTemp(): Uri {
        val timeStamp = SimpleDateFormat("yyyyMMdd_HHmmss").format(Date());
        val imageFileName = "JPEG_${timeStamp}_";
        val storageDir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES);
        val image = File.createTempFile(imageFileName, ".jpg", storageDir);
        image.deleteOnExit()
        pathToTempFile = "file:${image.getAbsolutePath()}"
        return Uri.fromFile(image);
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        val uri = Uri.parse(pathToTempFile)
        val output = MediaStore.Images.Media.getBitmap (getContentResolver(), uri)
        File(uri.getPath()).delete()
        pathToTempFile = null
        val stream = ByteArrayOutputStream()
        output.compress(Bitmap.CompressFormat.JPEG, 100, stream)
        send(stream.toByteArray())
    }
    private fun send(data: ByteArray) {
        Thread() {
            val socket = Socket()
            socket.use {
                try {
                    socket.connect(InetSocketAddress(InetAddress.getByName(ipField.getText().toString()), 50001))
                    val outputStream = socket.getOutputStream()
                    outputStream.use {
                        outputStream.write(data)
                        outputStream.flush()
                        outputStream.close();
                    }
                } catch(e: IOException) {
                    runOnUiThread { Toast.makeText(getBaseContext(), "Не удалось отправить", Toast.LENGTH_LONG).show() }
                }
            }

        }.start()
    }
}
