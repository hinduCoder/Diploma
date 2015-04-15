package com.vedroid.diplomaassistant

import android.app.Activity
import android.content.Intent
import android.graphics.Bitmap
import android.net.LocalSocket
import android.os.Bundle
import android.provider.MediaStore
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.Toast
import butterknife.bindView
import java.io.ByteArrayOutputStream
import java.io.IOException
import java.net.*


public class MainActivity : Activity() {

    private val ipField:EditText by bindView(R.id.id_field)
    private val sendButton: Button by bindView(R.id.send_button)

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        sendButton.setOnClickListener {onSendButtonClick()}
    }

    private fun onSendButtonClick() {
        val intent = Intent(MediaStore.ACTION_IMAGE_CAPTURE)
        startActivityForResult(intent, 1)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)

        val output = data!!.getExtras().get("data") as Bitmap
        val stream = ByteArrayOutputStream()
        output.compress(Bitmap.CompressFormat.JPEG, 100, stream)
        send(stream.toByteArray())
    }

    private fun send(data: ByteArray) {
        Thread() {
            val socket = DatagramSocket()
            socket.use {
                try {
                    socket.send(DatagramPacket(data, data.size(), InetAddress.getByName(ipField.getText().toString()), 50001))
                } catch(e: IOException) {
                    Toast.makeText(getBaseContext(), "Не удалось отправить", Toast.LENGTH_LONG).show()
                }
            }
        }.start()
    }
}
